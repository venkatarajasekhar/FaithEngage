using System;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginDtoFactory
	{
		DisplayUnitPluginDTO ConvertFromPlugin (DisplayUnitPlugin plugin, Guid? id = null);
	}
}

