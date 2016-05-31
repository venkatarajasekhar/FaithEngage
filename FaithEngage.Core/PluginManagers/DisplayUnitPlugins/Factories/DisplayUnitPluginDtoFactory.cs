using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
    public class DisplayUnitPluginDtoFactory
    {
        public DisplayUnitPluginDTO ConvertFromPlugin(DisplayUnitPlugin plugin)
        {
            var dto = new DisplayUnitPluginDTO () {
				Id = plugin.PluginId,
                AssemblyLocation = plugin.AssemblyLocation,
                FullName = plugin.FullName,
                PluginName = plugin.PluginName,
                PluginVersion = plugin.PluginVersion
            };
            return dto;
        }
    }
}

