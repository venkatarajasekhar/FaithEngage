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
            var fullPathSegments = source.FileInfo.FullName.Split (new [] { '/', '\\' });
            var pluginsPathSegments = _config.PluginsFolderPath.Split (new [] { '/', '\\' });
            var fullPluginPath = Path.Combine(Path.Combine (pluginsPathSegments), source.PluginId.ToString ());
            var relPath = Path.Combine (fullPathSegments).Replace (fullPluginPath, "");
            var dto = new PluginFileInfoDTO () { FileId = source.FileId, Name = source.FileInfo.Name, PluginId = source.PluginId, RelativePath = relPath};
            return dto;
		}
	}
}

