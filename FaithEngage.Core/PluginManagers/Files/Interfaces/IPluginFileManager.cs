using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace FaithEngage.Core.PluginManagers.Files.Interfaces
{
    public interface IPluginFileManager
    {
        IDictionary<Guid, PluginFileInfo> GetFilesForPlugin (Guid pluginId);
        PluginFileInfo GetFile (Guid fileId);
        void StoreFilesForPlugin (ZipArchive zipArchive, Guid pluginId);
        void RenameFile (Guid fileId, string newName);
        void DeleteFile (Guid fileId);
        void DeleteAllFilesForPlugin (Guid pluginId);
    }
}

