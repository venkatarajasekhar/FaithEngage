using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	/// <summary>
	/// The centralized repository for all DisplayUnitPlugins.
	/// </summary>
	public interface IDisplayUnitPluginRepoManager : IPluginRepoManager
	{
        /// <summary>
        /// Obtains all DisplayUnitPlugins.
        /// </summary>
        /// <returns>All DisplayUnitPlugins </returns>
		IEnumerable<DisplayUnitPlugin> GetAll ();
        /// <summary>
        /// Gets a DisplayUnitPlugin by its ID
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
		DisplayUnitPlugin GetById (Guid id);
	}
}

