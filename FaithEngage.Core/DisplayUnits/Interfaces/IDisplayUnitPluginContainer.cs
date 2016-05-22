using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    public interface IDisplayUnitPluginContainer
    {
        void Register(DisplayUnitPlugin plugin);
        DisplayUnitPlugin Resolve (string PluginId);

    }
}

