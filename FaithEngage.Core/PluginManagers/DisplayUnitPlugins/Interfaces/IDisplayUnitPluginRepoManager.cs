using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginRepoManager
	{
		void RegisterNew (DisplayUnitPlugin plugin);
		void UpdatePlugin (DisplayUnitPlugin plugin);
		void UninstallPlugin(Guid id);
		List<DisplayUnitPlugin> GetAll();
	}
}

