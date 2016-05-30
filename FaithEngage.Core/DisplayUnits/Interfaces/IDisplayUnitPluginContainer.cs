using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using System;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    public interface IDisplayUnitPluginContainer
    {
        void Register(DisplayUnitPlugin plugin);
        DisplayUnitPlugin Resolve (Guid PluginId);

    }
}

