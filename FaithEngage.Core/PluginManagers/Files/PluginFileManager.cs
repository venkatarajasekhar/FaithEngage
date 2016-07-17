using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.Config;
using System.Linq;
using FaithEngage.Core.RepoInterfaces;
namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileManager : IPluginFileManager
    {
        private readonly IPluginFileInfoRepository _repo;
        private readonly IConfigManager _config;
        private readonly IConverterFactory<PluginFileInfo, PluginFileInfoDTO> _dtoFac;
        private readonly IPluginFileInfoFactory _factory;

        private DirectoryInfo _tempFolder;
        private DirectoryInfo _pluginsFolder;
        public PluginFileManager (IConfigManager config, IPluginFileInfoRepository repo, IConverterFactory<PluginFileInfo, PluginFileInfoDTO> dtoFac, IPluginFileInfoFactory factory) 
        {
            _config = config;
            _repo = repo;
            _dtoFac = dtoFac;
            _factory = factory;

            _tempFolder = new DirectoryInfo (getOSSafePath(config.TempFolderPath));
            if (!_tempFolder.Exists) _tempFolder.Create ();
            _pluginsFolder = new DirectoryInfo (getOSSafePath(config.PluginsFolderPath));
            if (!_pluginsFolder.Exists) _pluginsFolder.Create ();
        }

        public void DeleteAllFilesForPlugin (Guid pluginId)
        {
            var folder = new DirectoryInfo (getBasePluginPath (pluginId));
            folder.Delete (true);
            _repo.DeleteAllFilesForPlugin (pluginId);

            //Insert try/catch blocks for repo calls and delete method. 
            //see https://msdn.microsoft.com/en-us/library/system.io.fileinfo.delete(v=vs.110).aspx
        }

        public void DeleteFile (Guid fileId)
        {
            var pFileInfo = GetFile (fileId);
            if (pFileInfo.FileInfo.Exists) pFileInfo.FileInfo.Delete ();
            _repo.DeleteFileRecord (fileId);

            //Insert try/catch blocks for repo calls and delete method. 
            //see https://msdn.microsoft.com/en-us/library/system.io.fileinfo.delete(v=vs.110).aspx
        }

        public IList<FileInfo> ExtractZipToTempFolder (ZipArchive zipArchive, Guid key)
        {
            var folder = _tempFolder.CreateSubdirectory (key.ToString ());
            zipArchive.ExtractToDirectory (folder.FullName);
            return folder.EnumerateFiles ().ToList ();

            //Insert try/catch blocks for repo calls and createSubdirctory method. 
        }

        public void FlushTempFolder ()
        {
            _tempFolder.EnumerateFiles ().ToList ().ForEach (p => p.Delete ());
            _tempFolder.EnumerateDirectories ().ToList ().ForEach (p => p.Delete ());
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
            var newFileName = Path.Combine (getBasePluginPath (file.PluginId), getOSSafePath (newRelativePath));
            var oldFileName = file.FileInfo.FullName;
            try {
                file.FileInfo = file.FileInfo.CopyTo (newFileName);
                File.Delete (oldFileName);
                var dto = _dtoFac.Convert (file);
                _repo.UpdateFile (dto);
            } catch (Exception ex) {
                //TODO: Come up with all the exception cases 
            }

        }

        private string getBasePluginPath (Guid pluginId)
        {
            return Path.Combine (_pluginsFolder.FullName, pluginId.ToString ());
        }

        private string getOSSafePath(string unsafePath){
            var segments = unsafePath.Split ('/', '\\');
            return Path.Combine (segments);
        }

        public void StoreFilesForPlugin (IList<FileInfo> files, Guid pluginId, bool overWrite = false)
        {
            foreach(var file in files)
            {
                var newPath = Path.Combine (getBasePluginPath (pluginId), file.Name);
                var savedFile = file.CopyTo (newPath, overWrite);
                var pfile = _factory.Create (savedFile, pluginId);
                var dto = _dtoFac.Convert (pfile);
                _repo.SaveFileInfo (dto);
            }
        }
    }
}

