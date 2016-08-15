using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.RepoInterfaces
{
    public interface IPluginRepository
    {
        Guid Register(PluginDTO plugin);
		void Update(PluginDTO plugin);
        void Delete(Guid pluginId);
        PluginDTO GetById(Guid pluginId);
        List<PluginDTO> GetAll();
    }
}

