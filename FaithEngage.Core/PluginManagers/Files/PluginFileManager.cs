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
			if (Directory.Exists(path)) 
				doFileAction(path, p => Directory.Delete(p, true));
            try{
                _repo.DeleteAllFilesForPlugin (pluginId);
            }catch(RepositoryException){
                return;
            }
        }

        public void DeleteFile (Guid fileId)
        {
            var pFileInfo = GetFile (fileId);
			if (pFileInfo.FileInfo.Exists) 
				doFileAction(pFileInfo, p => p.FileInfo.Delete());
            try{
                _repo.DeleteFileRecord (fileId);
			} catch (RepositoryException){}
        }

        public IList<FileInfo> ExtractZipToTempFolder (ZipArchive zipArchive, Guid key)
        {
            
			DirectoryInfo folder = doFileAction(key, p=>  _tempFolder.CreateSubdirectory(p.ToString()));
			doFileAction(folder, p => zipArchive.ExtractToDirectory(p.FullName));
			IList<FileInfo> list = doFileAction(folder, p=> p.EnumerateFiles("*",SearchOption.AllDirectories).ToList());
            return list;
        }

		public void FlushTempFolder(Guid key)
		{
			_tempFolder.EnumerateDirectories(key.ToString())
		   .ToList()
		   .ForEach(p =>
				{
					try
					{
						p.Delete(true);
					}
					catch (Exception ex)
					{
						throwFileException(ex, p.FullName);
					}
				});
        }

        public void FlushTempFolder ()
        {
			var dirs = doFileAction(_tempFolder, p=> p.EnumerateDirectories ());
            foreach(var dir in dirs){
                try {
                    dir.Delete (true);
                } catch (Exception) {
                    continue;
                }
            }
        }

        public PluginFileInfo GetFile (Guid fileId)
        {
            PluginFileInfoDTO dto = null;
            try {
                dto = _repo.GetFileInfo (fileId);
            } catch (RepositoryException ex) {
                throw new RepositoryException ("There was a problem obtaining the file record from the db.", ex);
            }
            if (dto == null) return null;
            var pfile = _factory.Convert (dto);
            return pfile;

        }

        public IDictionary<Guid, PluginFileInfo> GetFilesForPlugin (Guid pluginId)
        {
			IList<PluginFileInfoDTO> dtos;
			try
			{
				dtos = _repo.GetAllFilesForPlugin(pluginId);
			}
			catch (RepositoryException ex)
			{
				throw new RepositoryException("There was a problem obtaining the file records from the db.", ex);
			}
			if (dtos == null) return null;
            var dict = dtos.ToDictionary (p => p.FileId, p => _factory.Convert (p));
            return dict;
        }

        public void RenameFile (Guid fileId, string newRelativePath)
        {
            var file = GetFile (fileId);
			if (file == null) 
				throw new InvalidFileException(fileId.ToString(), $"The file with the id {fileId.ToString()} does not exist in either the db or on the filesystem.");
			var oldFileName = file.FileInfo.FullName;
			var newPath = _factory.GetRenamedPath(file, newRelativePath);
			var dirName = Path.GetDirectoryName(newPath);
			if (!Directory.Exists(dirName)) doFileAction(dirName, p=> Directory.CreateDirectory(p));
			file.FileInfo = doFileAction(newPath, p => file.FileInfo.CopyTo(newPath));
			doFileAction(oldFileName, p => File.Delete(p));
			var dto = _dtoFac.Convert(file);
			try
			{
				_repo.UpdateFile(dto);
			}
			catch (RepositoryException ex)
			{
				throw new RepositoryException("There was a problem updating the db.", ex);
			}

        }

        public void StoreFilesForPlugin (IList<FileInfo> files, Guid pluginId, bool overWrite = false)
        {
            var anchorDir = getCommonDir (files, 3);

            foreach(var file in files)
            {
                if(file.Exists){
                    var newPath = makeNewPath (anchorDir, file, pluginId);
                    var dirPath = Path.GetDirectoryName (newPath);
                    doFileAction(dirPath,p=> Directory.CreateDirectory (p));
                    var savedFile = doFileAction(file, p=> p.CopyTo(newPath, overWrite));
                    var pfile = _factory.Create (savedFile, pluginId);
                    var dto = _dtoFac.Convert (pfile);
                    _repo.SaveFileInfo (dto);
                }
            }
        }

        private string makeNewPath(DirectoryInfo anchorDir, FileInfo file, Guid pluginId)
        {
            var relPath = file.DirectoryName.Replace (anchorDir.FullName, "");
            relPath = (relPath != "" && relPath [0] == Path.DirectorySeparatorChar) 
                ? relPath.Remove (0, 1) : relPath;
            return Path.Combine (_factory.GetBasePluginPath (pluginId), relPath, file.Name);
        }

        private DirectoryInfo getCommonDir(IList<FileInfo> files, int maxDepth)
        {
            string currentDir = Path.GetPathRoot (files [0].FullName);
            if (files.All (p => p.FullName.Contains (_tempFolder.FullName))) {
                return _tempFolder;
            }

            for (int i = 0; i < files.Count;i++)
            {
                string commonDir;
                if(files.ElementAtOrDefault(i+1) != null){
                    commonDir = FindCommonDir (files [i].FullName,files [i + 1].FullName);
                }else{
                    commonDir = files [i].Directory.FullName;
                }

                if(currentDir == Path.GetPathRoot(files[i].FullName)){
                    currentDir = commonDir;
                }else if(currentDir == commonDir){
                    continue;
                }
                else{
                    currentDir = FindCommonDir (currentDir, commonDir);
                }
            }
            return new DirectoryInfo (currentDir);
        }

        private string FindCommonDir (string path1, string path2)
        {
            string p1 = File.Exists (path1) ? Path.GetDirectoryName (path1) : path1;
            string p2 = File.Exists (path2) ? Path.GetDirectoryName (path2) : path2;
            if (p1 == p2) return p1;
            var segs1 = p1.Split (Path.DirectorySeparatorChar);
            var segs2 = p2.Split (Path.DirectorySeparatorChar);

            if(segs1.Length > segs2.Length){
                string [] segs = new string [segs2.Length];
                Array.Copy (segs1, segs, segs2.Length);
                segs1 = segs;
            }else if(segs1.Length < segs2.Length){
                string [] segs = new string [segs1.Length];
                Array.Copy (segs2, segs, segs1.Length);
            }
            int index;

            var list = new List<string> ();

            for (index = 0; index < segs1.Length; index++) {
                if (segs1 [index] != segs2 [index]) break;
                list.Add (segs1 [index]);
            }

            if(list[0] == ""){ //In the case of a unix-based system
                list [0] = Path.DirectorySeparatorChar.ToString();
            }else if (Path.GetPathRoot(p1).Contains(list[0])) //In the case of windows-based system
			{
				list[0] += Path.DirectorySeparatorChar;
			}
				

            var commondir = Path.Combine (list.ToArray());
            return commondir;

        }


		private void throwFileException(Exception ex, object subject)
		{
			try { throw ex; }
			catch (IOException)
			{
				throw new PluginFileException($"IO Exception encountered regarding {subject.ToString()}", ex);
			}
			catch (UnauthorizedAccessException)
			{
				throw new PluginFileException($"Unauthorized Access on {subject.ToString()}", ex);
			}
			catch (SecurityException)
			{
				throw new PluginFileException($"Unauthorized Access on {subject.ToString()}", ex);
			}
		}

		private Tresult doFileAction<Tinput, Tresult>(Tinput source, Func<Tinput, Tresult> func)
		{
			Tresult result = default(Tresult);
			try
			{
				result = func(source);
			}
			catch (Exception ex)
			{
				throwFileException(ex, source);
			}
			return result;
		}

		private void doFileAction<Tinput>(Tinput source, Action<Tinput> func)
		{
			try
			{
				func(source);
			}
			catch (Exception ex)
			{
				throwFileException(ex, source);
			}
		}
    }
}

