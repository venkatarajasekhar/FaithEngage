using System;
using System.Threading.Tasks;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    public interface IDisplayUnitPluginRepository
    {
        bool CheckRegistration(string PluginId);
        void RegisterPlugin (DisplayUnitPluginDTO plugin);
        DisplayUnitPluginDTO GetById(string PluginId);
        void DeletePlugin(string PluginId);
    }
}

