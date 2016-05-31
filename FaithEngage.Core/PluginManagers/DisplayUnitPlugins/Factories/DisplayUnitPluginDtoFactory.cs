using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
    public class DisplayUnitPluginDtoFactory : IDisplayUnitPluginDtoFactory
    {
        public DisplayUnitPluginDTO ConvertFromPlugin(DisplayUnitPlugin plugin)
        {
			var dto = new DisplayUnitPluginDTO ();
			dto.Id = plugin.PluginId;
			dto.AssemblyLocation = plugin.AssemblyLocation;
			dto.FullName = plugin.FullName;
			dto.PluginName = plugin.PluginName;
			dto.PluginVersion = plugin.PluginVersion;
            return dto;
        }
    }
}

