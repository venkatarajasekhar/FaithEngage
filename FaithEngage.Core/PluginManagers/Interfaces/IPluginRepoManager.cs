using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
	public interface IPluginRepoManager
	{
        void RegisterNew(Plugin plugin, Guid pluginId);
		void UninstallPlugin(Guid id);
		void UpdatePlugin(Plugin plugin);
	}
}

