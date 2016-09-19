using System;
using FaithEngage.Core.Config;
using System.IO;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
	public class PluginFileInfoDTOFactory : IConverterFactory<PluginFileInfo, PluginFileInfoDTO>
	{

        private IConfigManager _config;
        public PluginFileInfoDTOFactory (IConfigManager config)
        {
            _config = config;
        }

        public PluginFileInfoDTO Convert(PluginFileInfo source)
		{
            var pluginsPath = Path.Combine(_config.PluginsFolderPath.Split (new [] { '/', '\\' }));
            var fullPluginsPath = new DirectoryInfo (pluginsPath).FullName;
            var indPluginPath = Path.Combine(Path.Combine (fullPluginsPath), source.PluginId.ToString ());
            if (!source.FileInfo.FullName.Contains (indPluginPath)) 
                throw new FactoryException (
                    $"PluginFileInfo {source.FileInfo.Name} must be in the specified plugins path: {indPluginPath}.");
            var relPath = source.FileInfo.FullName.Replace (indPluginPath, "");
            if (relPath [0] == Path.DirectorySeparatorChar) relPath = relPath.Substring (1);
            var dto = new PluginFileInfoDTO () { FileId = source.FileId, Name = source.FileInfo.Name, PluginId = source.PluginId, RelativePath = relPath};
            return dto;
		}
	}
}

