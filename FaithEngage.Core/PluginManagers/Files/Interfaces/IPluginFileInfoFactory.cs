using System;
using System.IO;
namespace FaithEngage.Core.PluginManagers.Files.Interfaces
{
    public interface IPluginFileInfoFactory : IConverterFactory<PluginFileInfoDTO, PluginFileInfo>
    {
        PluginFileInfo Create (FileInfo file, Guid pluginId);
    }
}

