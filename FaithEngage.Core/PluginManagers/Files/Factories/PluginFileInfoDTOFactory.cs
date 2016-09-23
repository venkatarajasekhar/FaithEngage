using System;
using FaithEngage.Core.Config;
using System.IO;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
	/// <summary>
	/// Converts PluginFileInfos into PluginFileInfoDTO objects.
	/// </summary>
	public class PluginFileInfoDTOFactory : IConverterFactory<PluginFileInfo, PluginFileInfoDTO>
	{

        private IConfigManager _config;
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FaithEngage.Core.PluginManagers.Files.Factories.PluginFileInfoDTOFactory"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
		public PluginFileInfoDTOFactory (IConfigManager config)
        {
            _config = config;
        }
		/// <summary>
		/// Convert the specified PluginFileInfo into a PluginFileInfoDTO
		/// </summary>
		/// <param name="source">Source.</param>
        public PluginFileInfoDTO Convert(PluginFileInfo source)
		{
            //Get the Plugins folder path from the config.
			var pluginsPath = Path.Combine(_config.PluginsFolderPath.Split (new [] { '/', '\\' }));
            //Get the full plugins folder path rather than the relative one
			//TODO: this might not be the best way to do it; current directory is often odd/different. Needs testing.
			var fullPluginsPath = new DirectoryInfo (pluginsPath).FullName;
            //Get the plugin path for the specific plugin
			var indPluginPath = Path.Combine(Path.Combine (fullPluginsPath), source.PluginId.ToString ());
            //If the plugin file info doesn't actually contain this plugin's path, it's invalid and throw.
			if (!source.FileInfo.FullName.Contains (indPluginPath)) 
                throw new FactoryException (
                    $"PluginFileInfo {source.FileInfo.Name} must be in the specified plugins path: {indPluginPath}.");
            //Get the relative path by removing the plugin path from the full path.
			var relPath = source.FileInfo.FullName.Replace (indPluginPath, "");
			//If the first character is the directory separator character (strip that off)
			if (relPath [0] == Path.DirectorySeparatorChar) relPath = relPath.Substring (1);
            //Create the dto 
			var dto = new PluginFileInfoDTO () 
			{ 
				FileId = source.FileId, 
				Name = source.FileInfo.Name, 
				PluginId = source.PluginId, 
				RelativePath = relPath
			};
            return dto;
		}
	}
}

