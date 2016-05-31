using System;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginFactory
	{
		DisplayUnitPlugin LoadPluginFromDto (DisplayUnitPluginDTO dto);
	}
}

