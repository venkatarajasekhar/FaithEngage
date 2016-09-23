using System;
using System.IO;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Files.Interfaces
{
    /// <summary>
	/// Converts PluginFileInfoDTOs to PluginFileInfos, as well as other helper functions related
	/// to plugin files.
    /// </summary>
	public interface IPluginFileInfoFactory : IConverterFactory<PluginFileInfoDTO, PluginFileInfo>
    {
        /// <summary>
		/// Creates a new PluginFileInfo
		/// </summary>
		/// <param name="file">File.</param>
		/// <param name="pluginId">Plugin identifier.</param>
		PluginFileInfo Create (FileInfo file, Guid pluginId);
		/// <summary>
		/// Renames a path of a PluginFileInfo to a new relative path (within its plugin directory).
		/// </summary>
		/// <returns>The renamed path.</returns>
		/// <param name="pluginFile">Plugin file.</param>
		/// <param name="newRelativePath">New relative path.</param>
		string GetRenamedPath(PluginFileInfo pluginFile, string newRelativePath);
        /// <summary>
        /// Gets the base plugin path for the plugin with the specified id.
        /// </summary>
        /// <returns>The base plugin path.</returns>
        /// <param name="pluginId">Plugin identifier.</param>
		string GetBasePluginPath (Guid pluginId);
		/// <summary>
		/// Gets the temp folder's DirectoryInfo.
		/// </summary>
		/// <value>The temp folder.</value>
		DirectoryInfo TempFolder { get; }
		/// <summary>
        /// Gets the plugins folder's DirectoryInfo.
        /// </summary>
        /// <value>The plugins folder.</value>
		DirectoryInfo PluginsFolder { get; }

    }
}

