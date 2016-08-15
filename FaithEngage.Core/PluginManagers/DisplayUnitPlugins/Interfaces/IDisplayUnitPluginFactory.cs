using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginFactory
	{
		DisplayUnitPlugin LoadPluginFromDto (PluginDTO dto);
		IEnumerable<DisplayUnitPlugin> LoadPluginsFromDtos (IEnumerable<PluginDTO> dtos);
	}
}

