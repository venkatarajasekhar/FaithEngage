using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.Config;
using System.Linq;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Exceptions;
using System.Security;

namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileManager : IPluginFileManager
    {
        private readonly IPluginFileInfoRepository _repo;
        private readonly IConverterFactory<PluginFileInfo, PluginFileInfoDTO> _dtoFac;
        private readonly IPluginFileInfoFactory _factory;
		private DirectoryInfo _tempFolder;
		private DirectoryInfo _pluginsFolder;
        public PluginFileManager (IPluginFileInfoRepository repo, IConverterFactory<PluginFileInfo, PluginFileInfoDTO> dtoFac, IPluginFileInfoFactory factory) 
        {
            _repo = repo;
            _dtoFac = dtoFac;
            _factory = factory;

			_tempFolder = _factory.TempFolder;
			_pluginsFolder = factory.PluginsFolder;
        }

        public void DeleteAllFilesForPlugin (Guid pluginId)
        {
            var path = _factory.GetBasePluginPath (pluginId);
			try
			{
				if (Directory.Exists(path)) Directory.Delete(path, true);
			}
			catch (IOException ex)
			{
				throw new PluginFileException(path + " was not able to be deleted.", ex);
			}
			catch (UnauthorizedAccessException ex)
			{
				throw new PluginFileException(path + " was not able to be deleted due to unauthorized access.", ex);
			}
            try{
                _repo.DeleteAllFilesForPlugin (pluginId);
            }catch(Exception){}
        }

        public void DeleteFile (Guid fileId)
        {
            var pFileInfo = GetFile (fileId);
			try
			{
				if (pFileInfo.FileInfo.Exists) pFileInfo.FileInfo.Delete();
			}
			catch (IOException ex)
			{
				throw new PluginFileException(pFileInfo.FileInfo.FullName + " was not able to be deleted.", ex);
			}
			catch (UnauthorizedAccessException ex)
			{
				throw new PluginFileException(pFileInfo.FileInfo.FullName + " was not able to be deleted due to unauthorized access.", ex);
			}
            try{
                _repo.DeleteFileRecord (fileId);
			} catch (RepositoryException){}
        }

        public IList<FileInfo> ExtractZipToTempFolder (ZipArchive zipArchive, Guid key)
        {
            IList<FileInfo> list = null;
			DirectoryInfo folder = null;
			try
			{
				folder = _tempFolder.CreateSubdirectory(key.ToString());
				zipArchive.ExtractToDirectory(folder.FullName);
				list = folder.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
			}
			catch (ArgumentException)
			{
				return new List<FileInfo>();
			}
			catch (IOException ex)
			{
				throw new PluginFileException("Could not create subfolder of " + _tempFolder.FullName + ".", ex);
			}
			catch (SecurityException ex)
			{
				throw new PluginFileException("Inadequate permissions to enumerate files on " + folder.FullName + ".", ex);
			}
            return list;
        }

        public void FlushTempFolder (Guid key)
        {
            _tempFolder.EnumerateDirectories (key.ToString ()).ToList ().ForEach (p => p.Delete(true));
        }

        public void FlushTempFolder ()
        {
            _tempFolder.EnumerateDirectories ().ToList ().ForEach (p => p.Delete (true));
            //Insert try/catch blocks for repo calls and delete method. 
            //see https://msdn.microsoft.com/en-us/library/system.io.fileinfo.delete(v=vs.110).aspx
        }

        public PluginFileInfo GetFile (Guid fileId)
        {
            var dto = _repo.GetFileInfo (fileId);
            return _factory.Convert (dto);
            //Insert try/catch block for repo call
        }

        public IDictionary<Guid, PluginFileInfo> GetFilesForPlugin (Guid pluginId)
        {
            var dtos = _repo.GetAllFilesForPlugin (pluginId);
            var dict = dtos.ToDictionary (p => p.FileId, p => _factory.Convert (p));
            return dict;
        }

        public void RenameFile (Guid fileId, string newRelativePath)
        {
            var file = GetFile (fileId);
			var newFile = _factory.Rename(file, newRelativePath);
			var dto = _dtoFac.Convert(newFile);
			_repo.UpdateFile(dto);
        }

        public void StoreFilesForPlugin (IList<FileInfo> files, Guid pluginId, bool overWrite = false)
        {
            foreach(var file in files)
            {
                if(file.Exists){
                    var newPath = Path.Combine (_factory.GetBasePluginPath (pluginId), file.Name);
                    var dirPath = Path.GetDirectoryName (newPath);
					var parentDir = Directory.CreateDirectory (dirPath);
                    var savedFile = file.CopyTo (newPath, overWrite);
                    var pfile = _factory.Create (savedFile, pluginId);
                    var dto = _dtoFac.Convert (pfile);
                    _repo.SaveFileInfo (dto);
                }
            }
        }
		private void throwFileException(Exception ex, string pluginPath)
		{
			try { throw ex; }
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (ArgumentException)
			{
			}
		}
    }
}

