using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
	public interface IPluginRepoManager
	{
        Guid RegisterNew(Plugin plugin);
		void UninstallPlugin(Guid id);
		void UpdatePlugin(Plugin plugin);
	}
}

