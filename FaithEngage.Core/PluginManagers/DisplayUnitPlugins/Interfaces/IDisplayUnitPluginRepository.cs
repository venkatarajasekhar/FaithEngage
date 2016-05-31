using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces
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

