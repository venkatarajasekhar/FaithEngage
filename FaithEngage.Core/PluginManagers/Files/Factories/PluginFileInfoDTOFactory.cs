using System;
using FaithEngage.Core.Config;
using System.IO;
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
            var relPath = source.FileInfo.FullName.Replace (indPluginPath, "");
            var dto = new PluginFileInfoDTO () { FileId = source.FileId, Name = source.FileInfo.Name, PluginId = source.PluginId, RelativePath = relPath};
            return dto;
		}
	}
}

