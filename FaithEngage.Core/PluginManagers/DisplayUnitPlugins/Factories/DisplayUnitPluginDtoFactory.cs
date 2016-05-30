using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
    public class DisplayUnitPluginDtoFactory
    {
        public DisplayUnitPluginDTO ConvertFromPlugin(DisplayUnitPlugin plugin, Guid? id = null)
        {
            var dto = new DisplayUnitPluginDTO () {
                Id = id,
                AssemblyLocation = plugin.AssemblyLocation,
                FullName = plugin.FullName,
                PluginName = plugin.PluginName,
                PluginVersion = plugin.PluginVersion
            };
            return dto;
        }
    }
}

