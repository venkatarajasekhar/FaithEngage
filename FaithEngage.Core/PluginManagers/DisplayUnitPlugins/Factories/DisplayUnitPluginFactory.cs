using System;
using System.Reflection;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;


namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
    public class DisplayUnitPluginFactory : IDisplayUnitPluginFactory
    {
        public DisplayUnitPlugin LoadPluginFromDto(DisplayUnitPluginDTO dto)
        {
			DisplayUnitPlugin plugin = null;
			try {
				var assembly = Assembly.LoadFrom (dto.AssemblyLocation);
				var type = assembly.GetType (dto.FullName);
				var ctor = type.GetConstructor (new Type[]{ });
				plugin = (DisplayUnitPlugin)ctor.Invoke (new object[]{ });
				plugin.PluginId = dto.Id;
			} catch{
				return null;
			}
            return plugin;
        }

		public IEnumerable<DisplayUnitPlugin> LoadPluginsFromDtos(IEnumerable<DisplayUnitPluginDTO> dtos)
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

