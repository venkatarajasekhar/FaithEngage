using System;
using System.IO;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Files.Interfaces
{
    public interface IPluginFileInfoFactory : IConverterFactory<PluginFileInfoDTO, PluginFileInfo>
    {
        PluginFileInfo Create (FileInfo file, Guid pluginId);
		string GetRenamedPath(PluginFileInfo pluginFile, string newRelativePath);
        string GetBasePluginPath (Guid pluginId);
		DirectoryInfo TempFolder { get; }
		DirectoryInfo PluginsFolder { get; }

    }
}

