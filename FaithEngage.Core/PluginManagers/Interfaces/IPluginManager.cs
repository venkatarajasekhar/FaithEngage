using System;
using System.IO.Compression;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.Collections.Generic;
using System.IO;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
    /// <summary>
    /// Managers the installation, uninstallation, and initialization of plugins.
    /// </summary>
	public interface IPluginManager
    {
		/// <summary>
		/// Installs one or more plugins from a zip archive. 
		/// </summary>
		/// <param name="zipFile">The ziparchive containing any dlls, files, and the json manifest of plugins. </param>
		int Install(ZipArchive zipFile);
        /// <summary>
        /// Installs a plugin of the given type along with any associated files for the plugin.
        /// </summary>
        /// <param name="files">Files.</param>
        /// <typeparam name="TPlugin">The Plugin Type</typeparam>
		void Install<TPlugin> (IList<FileInfo> files = null) where TPlugin : Plugin, new();
		/// <summary>
		/// Uninstalls a plugin identified by the give id.
		/// </summary>
		/// <param name="pluginId">Plugin identifier.</param>
		void Uninstall(Guid pluginId);
		/// <summary>
		/// Initializes all installed plugins (to be run at boot).
		/// </summary>
		void InitializeAllPlugins();
        /// <summary>
        /// Checks whether the identified plugin has been registered.
        /// </summary>
        /// <returns><c>true</c>, if registered, <c>false</c> otherwise.</returns>
        /// <param name="pluginId">Plugin identifier.</param>
		bool CheckRegistered (Guid pluginId);
        /// <summary>
        /// Checks whether the identified plugin type has been registered.
        /// </summary>
        /// <returns><c>true</c>, if plugin has been registerd, <c>false</c> otherwise.</returns>
        /// <typeparam name="TPlugin">The 1st type parameter.</typeparam>
		bool CheckRegistered<TPlugin> () where TPlugin : Plugin;
    }
}

