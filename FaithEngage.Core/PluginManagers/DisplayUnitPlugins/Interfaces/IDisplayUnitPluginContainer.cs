using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using System;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
    /// <summary>
    /// A centralized container to keep all DisplayUnitPlugin types in memory, as needed.
    /// </summary>
	public interface IDisplayUnitPluginContainer
    {
        /// <summary>
        /// Register the specified plugin for later access.
        /// </summary>
        /// <param name="plugin">Plugin.</param>
		void Register(DisplayUnitPlugin plugin);
        /// <summary>
        /// Resolve the specified PluginId to a DisplayUnitPlugin
        /// </summary>
        /// <param name="PluginId">Plugin identifier.</param>
		DisplayUnitPlugin Resolve (Guid PluginId);
    }
}

