using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.RepoInterfaces
{
    public interface IDisplayUnitPluginRepository
    {
        Guid Register(DisplayUnitPluginDTO plugin);
		void Update(DisplayUnitPluginDTO plugin);
        void Delete(Guid pluginId);
        DisplayUnitPluginDTO GetById(Guid pluginId);
        List<DisplayUnitPluginDTO> GetAll();
    }
}

