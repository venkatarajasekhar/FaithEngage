using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
{
	public interface IDisplayUnitPluginRepoManager : IPluginRepoManager
	{
        IEnumerable<DisplayUnitPlugin> GetAll ();
        DisplayUnitPlugin GetById (Guid id);
	}
}

