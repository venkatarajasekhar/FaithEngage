using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;

namespace FaithEngage.Core.PluginManagers.Files.Interfaces
{
    /// <summary>
    /// Provides centralized access to the file resources for plugins.
    /// </summary>
	public interface IPluginFileManager
    {
        /// <summary>
        /// Obtains a Dictionary of PluginFileInfos for a specified plugin.
        /// </summary>
        /// <returns>The files for plugin, where the key is the fileId and the value is the
		/// PluginFileInfo</returns>
        /// <param name="pluginId">Plugin identifier.</param>
		IDictionary<Guid, PluginFileInfo> GetFilesForPlugin (Guid pluginId);
        /// <summary>
        /// Obtains a single file by its id.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileId">File identifier.</param>
		PluginFileInfo GetFile (Guid fileId);
        /// <summary>
        /// Receives a list of Files and will store them for the specified plugin within the
		/// application's plugin file directory.
        /// </summary>
        /// <param name="files">Files.</param>
        /// <param name="pluginId">Plugin identifier.</param>
        /// <param name="overWrite">If set to <c>true</c> over write.</param>
		void StoreFilesForPlugin (IList<FileInfo> files, Guid pluginId, bool overWrite = false);
        /// <summary>
        /// Renames a file with the specified ID.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="newName">New name.</param>
		void RenameFile (Guid fileId, string newName);
        /// <summary>
        /// Deletes the file with the specified ID.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
		void DeleteFile (Guid fileId);
        /// <summary>
        /// Deletes all files for a specified plugin.
        /// </summary>
        /// <param name="pluginId">Plugin identifier.</param>
		void DeleteAllFilesForPlugin (Guid pluginId);
        /// <summary>
        /// Extracts a zip folder and places all the files in a temp folder with the specified
		/// key, returning a list of FileInfos that have been stored.
        /// </summary>
        /// <returns>The zip to temp folder.</returns>
        /// <param name="zipArchive">Zip archive.</param>
        /// <param name="key">Key.</param>
		IList<FileInfo> ExtractZipToTempFolder (ZipArchive zipArchive, Guid key);
        /// <summary>
        /// Deletes all subdirectories of the temp folder.
        /// </summary>
		void FlushTempFolder ();
        /// <summary>
        /// Deletes the tempfolder identified by the given ID.
        /// </summary>
        /// <param name="key">Key.</param>
		void FlushTempFolder (Guid key);

    }
}

