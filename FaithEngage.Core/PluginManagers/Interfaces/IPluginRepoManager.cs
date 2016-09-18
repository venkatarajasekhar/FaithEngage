using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
	public interface IPluginRepoManager
	{
        void RegisterNew(Plugin plugin, Guid pluginId);
		void UninstallPlugin(Guid id);
		void UpdatePlugin(Plugin plugin);
		IDictionary<Guid, Plugin> GetAllPlugins();
        bool CheckRegistered (Guid pluginId);
        bool CheckRegistered<TPlugin> () where TPlugin : Plugin;
	}
}

