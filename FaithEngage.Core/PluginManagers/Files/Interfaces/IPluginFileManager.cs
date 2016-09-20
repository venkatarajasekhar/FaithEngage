using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;

namespace FaithEngage.Core.PluginManagers.Files.Interfaces
{
    public interface IPluginFileManager
    {
        IDictionary<Guid, PluginFileInfo> GetFilesForPlugin (Guid pluginId);
        PluginFileInfo GetFile (Guid fileId);
        void StoreFilesForPlugin (IList<FileInfo> files, Guid pluginId, bool overWrite = false);
        void RenameFile (Guid fileId, string newName);
        void DeleteFile (Guid fileId);
        void DeleteAllFilesForPlugin (Guid pluginId);
        IList<FileInfo> ExtractZipToTempFolder (ZipArchive zipArchive, Guid key);
        void FlushTempFolder ();
        void FlushTempFolder (Guid key);

    }
}

