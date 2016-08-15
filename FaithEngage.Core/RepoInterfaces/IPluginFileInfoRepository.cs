using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.Files;

namespace FaithEngage.Core.RepoInterfaces
{
    public interface IPluginFileInfoRepository
    {
        PluginFileInfoDTO GetFileInfo (Guid fileId);
        IList<PluginFileInfoDTO> GetAllFilesForPlugin (Guid pluginId);
        void DeleteFileRecord (Guid fileId);
        void DeleteAllFilesForPlugin (Guid pluginId);
        void UpdateFile (PluginFileInfoDTO dto);
        void SaveFileInfo (PluginFileInfoDTO dto);
    }
}

