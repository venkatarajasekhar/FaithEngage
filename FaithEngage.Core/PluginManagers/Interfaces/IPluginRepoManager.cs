using System;
using System.Collections.Generic;
using System.CodeDom;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
	/// <summary>
	/// Provides access to plugins stored in the respository.
	/// </summary>
	public interface IPluginRepoManager
	{
        /// <summary>
        /// Registers a new plugin to the db.
        /// </summary>
        /// <param name="plugin">Plugin.</param>
        /// <param name="pluginId">Plugin identifier.</param>
		void RegisterNew(Plugin plugin, Guid pluginId);
		/// <summary>
		/// Removes a plugin specified by the id from the db.
		/// </summary>
		/// <param name="id">Identifier.</param>
		void UninstallPlugin(Guid id);
		/// <summary>
		/// Updates a plugin already in the db.
		/// </summary>
		/// <param name="plugin">Plugin.</param>
		void UpdatePlugin(Plugin plugin);
		/// <summary>
		/// Obtains a dictionary of all currently installed plugins from the db.
		/// The dictionary key is the Plugin id.
		/// </summary>
		/// <returns>The all plugins.</returns>
		IDictionary<Guid, Plugin> GetAllPlugins();
        /// <summary>
        /// Checks whether the specified Plugin has been registerd into the repository.
        /// </summary>
        /// <returns><c>true</c>, if registered, <c>false</c> otherwise.</returns>
        /// <param name="pluginId">Plugin identifier.</param>
		bool CheckRegistered (Guid pluginId);
		/// <summary>
		/// Checks whether the specified Plugin type has been registerd into the repository.
		/// </summary>
		/// <returns><c>true</c>, if registered, <c>false</c> otherwise.</returns>
		/// <typeparam name="TPlugin">The plugin type</typeparam>
        bool CheckRegistered<TPlugin> () where TPlugin : Plugin;
	}
}

