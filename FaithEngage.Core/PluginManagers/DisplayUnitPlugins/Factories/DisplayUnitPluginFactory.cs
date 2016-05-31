using System;
using System.Reflection;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;


namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
    public class DisplayUnitPluginFactory : IDisplayUnitPluginFactory
    {
        public DisplayUnitPlugin LoadPluginFromDto(DisplayUnitPluginDTO dto)
        {
            var assembly = Assembly.LoadFrom (dto.AssemblyLocation);
            var type = assembly.GetType (dto.FullName);
            var ctor = type.GetConstructor (new Type[]{ });
            var plugin = ctor.Invoke (new object[]{ });

            return plugin as DisplayUnitPlugin;
        }
    }
}

