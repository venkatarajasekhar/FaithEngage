using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginFactory
	{
		DisplayUnitPlugin LoadPluginFromDto (DisplayUnitPluginDTO dto);
		IEnumerable<DisplayUnitPlugin> LoadPluginsFromDtos (IEnumerable<DisplayUnitPluginDTO> dtos);
	}
}

