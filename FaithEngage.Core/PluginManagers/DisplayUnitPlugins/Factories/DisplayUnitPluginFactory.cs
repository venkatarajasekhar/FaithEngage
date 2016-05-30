using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using System.Reflection;

namespace FaithEngage.Core
{
    public class DisplayUnitPluginFactory
    {
        DisplayUnitPlugin LoadPluginFromDto(DisplayUnitPluginDTO dto)
        {
            var assembly = Assembly.LoadFrom (dto.AssemblyLocation);
            var type = assembly.GetType (dto.FullName);
            var ctor = type.GetConstructor (new Type[]{ });
            var plugin = ctor.Invoke (new object[]{ });

            return plugin as DisplayUnitPlugin;
        }
    }
}

