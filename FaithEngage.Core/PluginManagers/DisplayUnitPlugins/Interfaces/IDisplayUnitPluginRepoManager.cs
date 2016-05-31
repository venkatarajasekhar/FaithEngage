using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginRepoManager
	{
		Guid RegisterNew (DisplayUnitPlugin plugin);
		void UpdatePlugin (DisplayUnitPlugin plugin);
		void UninstallPlugin(Guid id);
		IEnumerable<DisplayUnitPlugin> GetAll();
		DisplayUnitPlugin GetById(Guid id);
	}
}

