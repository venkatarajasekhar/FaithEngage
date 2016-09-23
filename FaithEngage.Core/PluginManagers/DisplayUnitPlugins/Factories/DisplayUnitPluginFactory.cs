using System;
using System.Reflection;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;


namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
    /// <summary>
    /// This converts a PluginDTOs to DisplayUnitPlugins.
    /// </summary>
	public class DisplayUnitPluginFactory : IDisplayUnitPluginFactory
    {
        /// <summary>
        /// Takes a pluginDTO and converts it to a DisplayUnitPlugin. 
        /// </summary>
        /// <returns>The new plugin converted from the dto. Null, if an exception is encountered
		/// or the type is not a known DisplayUnitType.</returns>
        /// <param name="dto">Dto.</param>
		public DisplayUnitPlugin LoadPluginFromDto(PluginDTO dto)
        {
			//Return null if the dto is not for a display unit.
			if (dto.PluginType != PluginTypeEnum.DisplayUnit) return null;
			DisplayUnitPlugin plugin = null;
			//TODO: Check if plugin type is already loaded.
			try {
				//Get the assembly referenced by filePath
				var assembly = Assembly.LoadFrom (dto.AssemblyLocation);
				//Get the Type from the assembly associated with the dto
				var type = assembly.GetType (dto.FullName);
				//Get the constructor for the type
				var ctor = type.GetConstructor (new Type[]{ });
				//Invoke the constructor and caste it to a DisplayUnitPlugin.
				plugin = (DisplayUnitPlugin)ctor.Invoke (new object[]{ });
				//Assign the plugin's ID as the dto's ID.
				plugin.PluginId = dto.Id;
			} catch{//If any exception is encountered, return null.
				return null;
			}
            return plugin;
        }
		/// <summary>
		/// Converts an ienumerable of PluginDTOs to an ienumerable of DisplayUnitPlugins
		/// </summary>
		/// <returns>An IEnumerable of DisplayUnitPlugins</returns>
		/// <param name="dtos">The dtos to convert.</param>
		public IEnumerable<DisplayUnitPlugin> LoadPluginsFromDtos(IEnumerable<PluginDTO> dtos)
		{
			foreach (var dto in dtos) {
				var du = this.LoadPluginFromDto (dto);
				if (du != null) {
					yield return du;
				} else {
					continue;
				}
			}
		}
    }
}

