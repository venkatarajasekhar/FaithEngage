using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using System.Linq;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Exceptions;
using System.Security;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Files
{
	/// <summary>
	/// Controls the access, storage, updating, and deletion of files related to 
	/// plugings.
	/// </summary>
	public class PluginFileManager : IPluginFileManager
	{
		private readonly IPluginFileInfoRepository _repo;
		private readonly IConverterFactory<PluginFileInfo, PluginFileInfoDTO> _dtoFac;
		private readonly IPluginFileInfoFactory _factory;
		private DirectoryInfo _tempFolder;
		private DirectoryInfo _pluginsFolder;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginManagers.Files.PluginFileManager"/> class.
		/// </summary>
		/// <param name="repo">PluginFileInfoRepository</param>
		/// <param name="dtoFac">DTO Factory</param>
		/// <param name="factory">PluginFileInfoFactory</param>
		public PluginFileManager (IPluginFileInfoRepository repo, IConverterFactory<PluginFileInfo, PluginFileInfoDTO> dtoFac, IPluginFileInfoFactory factory)
		{
			_repo = repo;
			_dtoFac = dtoFac;
			_factory = factory;

			_tempFolder = _factory.TempFolder;
			_pluginsFolder = factory.PluginsFolder;
		}
		/// <summary>
		/// Deletes the entire directory for a given plugin.
		/// </summary>
		/// <param name="pluginId">Plugin identifier.</param>
		public void DeleteAllFilesForPlugin (Guid pluginId)
		{
			//Get the plugin path
			var path = _factory.GetBasePluginPath (pluginId);
			//If it exists, delete it, recursively
			if (Directory.Exists (path))
				doFileAction (path, p => Directory.Delete (p, true));
			try {
				//Delete the records for all files associated with the specified plugin in the repo.
				_repo.DeleteAllFilesForPlugin (pluginId);
			} catch (RepositoryException) { //fail silently
				return;
			}
		}
		/// <summary>
		/// Deletes a single file, identified by its id.
		/// </summary>
		/// <param name="fileId">File identifier.</param>
		public void DeleteFile (Guid fileId)
		{
			//Get the file from the repo
			var pFileInfo = GetFile (fileId);
			//If it actually exists...
			if (pFileInfo.FileInfo.Exists)
				//Delete it.
				doFileAction (pFileInfo, p => p.FileInfo.Delete ());
			try {
				//Then delete the file record.
				_repo.DeleteFileRecord (fileId);
			} catch (RepositoryException) { /*fail silently */}
		}

		/// <summary>
		/// Extracts the zip to temp folder, identified by the specified key.
		/// </summary>
		/// <returns>The zip to temp folder.</returns>
		/// <param name="zipArchive">Zip archive.</param>
		/// <param name="key">Key.</param>
		public IList<FileInfo> ExtractZipToTempFolder (ZipArchive zipArchive, Guid key)
		{
			//Create a new temp folder with key as its name.
			DirectoryInfo folder = doFileAction (key, p => _tempFolder.CreateSubdirectory (p.ToString ()));
			//Extract the ziparchive to that temp folder
			doFileAction (folder, p => zipArchive.ExtractToDirectory (p.FullName));
			//Enumerate all the files in that directory structure into a list
			IList<FileInfo> list = doFileAction (folder, p => p.EnumerateFiles ("*", SearchOption.AllDirectories).ToList ());
			return list;
		}

		/// <summary>
		/// Deletes the temp folder specified.
		/// </summary>
		/// <param name="key">Key.</param>
		public void FlushTempFolder (Guid key)
		{
			//Get the subfolder of the temp folder has a name of the key.
			var folder = _tempFolder.EnumerateDirectories (key.ToString ()).FirstOrDefault ();
			//If such a folder exists...
			if (folder != null) {
				try {
					//Delete it.
					folder.Delete (true);
				} catch (Exception ex) {//Otherwise...
					//Throw.
					throwFileException (ex, folder.FullName);
				}
			}
		}
		/// <summary>
		/// Deletes all temp folders within the master temp folder.
		/// </summary>
		public void FlushTempFolder ()
		{
			//Get all subfolders of the master temp folder.
			var dirs = doFileAction (_tempFolder, p => p.EnumerateDirectories ());
			//For each one...
			foreach (var dir in dirs) {
				try {
					//Delete it
					dir.Delete (true);
				} catch (Exception) {//On exception, try the next one.
					continue;
				}
			}
		}
		/// <summary>
		/// Gets a file from the repository, identified by its id.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileId">File identifier.</param>
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
		/// <summary>
		/// Gets all files for a plugin.
		/// </summary>
		/// <returns>The files for plugin.</returns>
		/// <param name="pluginId">Plugin identifier.</param>
		public IDictionary<Guid, PluginFileInfo> GetFilesForPlugin (Guid pluginId)
		{
			IList<PluginFileInfoDTO> dtos;
			try {
				dtos = _repo.GetAllFilesForPlugin (pluginId);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem obtaining the file records from the db.", ex);
			}
			if (dtos == null) return null;
			var dict = dtos.ToDictionary (p => p.FileId, p => _factory.Convert (p));
			return dict;
		}
		/// <summary>
		/// Renames the file specified by the id to the new path and nane, relative to the plugin folder's
		/// path.
		/// </summary>
		/// <param name="fileId">File identifier.</param>
		/// <param name="relativePathName">New relative path.</param>
		public void RenameFile (Guid fileId, string relativePathName)
		{
			//Obtain the file from the repo
			var file = GetFile (fileId);
			//Throw an exception if it couldn't be obtained.
			if (file == null)
				throw new InvalidFileException (fileId.ToString (), $"The file with the id {fileId.ToString ()} does not exist in either the db or on the filesystem.");
			//Obtain the original full path
			var oldFileName = file.FileInfo.FullName;
			//Create the new, renamed path
			var newPath = _factory.GetRenamedPath (file, relativePathName);
			//Check if all the directories exist to move the file to that path
			var dirName = Path.GetDirectoryName (newPath);
			//If not, then create the directory structure needed.
			if (!Directory.Exists (dirName)) doFileAction (dirName, p => Directory.CreateDirectory (p));
			//Copy the file to that new path.
			file.FileInfo = doFileAction (newPath, p => file.FileInfo.CopyTo (newPath));
			//Delete the original file.
			doFileAction (oldFileName, p => File.Delete (p));
			//convert the new PluginFileInfo (using the new path and name) to a dto
			var dto = _dtoFac.Convert (file);
			try {
				//Save that dto to the repo
				_repo.UpdateFile (dto);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem updating the db.", ex);
			}

		}
		/// <summary>
		/// Stores a list of files for a plugin so that the files can be later retrieved.
		/// </summary>
		/// <param name="files">Files.</param>
		/// <param name="pluginId">Plugin identifier.</param>
		/// <param name="overWrite">If set to <c>true</c> overwrites any existing files.</param>
		public void StoreFilesForPlugin (IList<FileInfo> files, Guid pluginId, bool overWrite = false)
		{
			//Obtain the common directory for all of the files given (so that relative paths can be
			//created
			var anchorDir = getCommonDir (files);
			//Loop through the files. For each one...
			foreach (var file in files) {
				//Refresh the fileinfo and check if it exists.
				file.Refresh ();
				if (file.Exists) {
					//Create a new path for the file using the anchor directory, plugin id, and file
					var newPath = makeNewPath (anchorDir, file, pluginId);
					//Get the parent directory of the new file path
					var dirPath = Path.GetDirectoryName (newPath);
					//Create the directory (if it doesn't exist)
					doFileAction (dirPath, p => Directory.CreateDirectory (p));
					//Copy the file to that new path
					var savedFile = doFileAction (file, p => p.CopyTo (newPath, overWrite));
					//Create a new PluginFileInfo for that new file
					var pfile = _factory.Create (savedFile, pluginId);
					//Convert that PluginFileInfo to a dto
					var dto = _dtoFac.Convert (pfile);
					//Save that dto to the repository
					_repo.SaveFileInfo (dto);
				}
			}
		}
		/// <summary>
		/// Makes the new path to where the given file will be copied.
		/// </summary>
		/// <returns>The new path.</returns>
		/// <param name="anchorDir">The common root directory</param>
		/// <param name="file">File.</param>
		/// <param name="pluginId">Plugin identifier.</param>
		private string makeNewPath (DirectoryInfo anchorDir, FileInfo file, Guid pluginId)
		{
			//Cut the anchor directory's path out of the file's parent directory's full path --> relative path
			var relPath = file.DirectoryName.Replace (anchorDir.FullName, "");
			/*If relpath is not an empty string and it's first character is a directory separator
			 * remove the first character*/
			relPath = (relPath != "" && relPath [0] == Path.DirectorySeparatorChar)
				? relPath.Remove (0, 1) : relPath;
			//Return a path composed of the base plugin path, the relative path, and the files name.
			return Path.Combine (_factory.GetBasePluginPath (pluginId), relPath, file.Name);
		}

		private DirectoryInfo getCommonDir (IList<FileInfo> files)
		{
			string currentDir = Path.GetPathRoot (files [0].FullName);

			//var plugTempFolder = $"{_tempFolder.FullName}{Path.DirectorySeparatorChar}{pluginId}";
			//if (files.All (p => p.FullName.Contains (_tempFolder.FullName))) {
			//    return _tempFolder;
			//}

			for (int i = 0; i < files.Count; i++) {
				string commonDir;
				if (files.ElementAtOrDefault (i + 1) != null) {
					commonDir = FindCommonDir (files [i].FullName, files [i + 1].FullName);
				} else {
					commonDir = files [i].Directory.FullName;
				}

				if (currentDir == Path.GetPathRoot (files [i].FullName)) {
					currentDir = commonDir;
				} else if (currentDir == commonDir) {
					continue;
				} else {
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

			if (segs1.Length > segs2.Length) {
				string [] segs = new string [segs2.Length];
				Array.Copy (segs1, segs, segs2.Length);
				segs1 = segs;
			} else if (segs1.Length < segs2.Length) {
				string [] segs = new string [segs1.Length];
				Array.Copy (segs2, segs, segs1.Length);
			}
			int index;

			var list = new List<string> ();

			for (index = 0; index < segs1.Length; index++) {
				if (segs1 [index] != segs2 [index]) break;
				list.Add (segs1 [index]);
			}

			if (list [0] == "") { //In the case of a unix-based system
				list [0] = Path.DirectorySeparatorChar.ToString ();
			} else if (Path.GetPathRoot (p1).Contains (list [0])) //In the case of windows-based system
			 {
				list [0] += Path.DirectorySeparatorChar;
			}


			var commondir = Path.Combine (list.ToArray ());
			return commondir;

		}


		private void throwFileException (Exception ex, object subject)
		{
			try { throw ex; } catch (IOException) {
				throw new PluginFileException ($"IO Exception encountered regarding {subject.ToString ()}", ex);
			} catch (UnauthorizedAccessException) {
				throw new PluginFileException ($"Unauthorized Access on {subject.ToString ()}", ex);
			} catch (SecurityException) {
				throw new PluginFileException ($"Unauthorized Access on {subject.ToString ()}", ex);
			}
		}

		private Tresult doFileAction<Tinput, Tresult> (Tinput source, Func<Tinput, Tresult> func)
		{
			Tresult result = default (Tresult);
			try {
				result = func (source);
			} catch (Exception ex) {
				throwFileException (ex, source);
			}
			return result;
		}

		private void doFileAction<Tinput> (Tinput source, Action<Tinput> func)
		{
			try {
				func (source);
			} catch (Exception ex) {
				throwFileException (ex, source);
			}
		}
	}
}

