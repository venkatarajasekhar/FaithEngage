using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	/// <summary>
	/// This converts a PluginDTOs to DisplayUnitPlugins.
	/// </summary>
	public interface IDisplayUnitPluginFactory
	{
		/// <summary>
		/// Takes a pluginDTO and converts it to a DisplayUnitPlugin. 
		/// </summary>
		/// <returns>The new plugin converted from the dto. Null, if an exception is encountered
		/// or the type is not a known DisplayUnitType.</returns>
		/// <param name="dto">Dto.</param>
		DisplayUnitPlugin LoadPluginFromDto (PluginDTO dto);
		/// <summary>
		/// Converts an ienumerable of PluginDTOs to an ienumerable of DisplayUnitPlugins
		/// </summary>
		/// <returns>An IEnumerable of DisplayUnitPlugins</returns>
		/// <param name="dtos">The dtos to convert.</param>
		IEnumerable<DisplayUnitPlugin> LoadPluginsFromDtos (IEnumerable<PluginDTO> dtos);
	}
}

