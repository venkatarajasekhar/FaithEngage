using System;
using System.Collections.Generic;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
    public class DisplayUnitPluginBootstrapper : IBootstrapper
	{
        public BootPriority BootPriority {
            get {
                return BootPriority.Last;
            }
        }

        public void Execute(IAppFactory factory)
		{
            //Grab the DisplayUnitPluginContainer
			var pluginContainer = factory.GetOther<IDisplayUnitPluginContainer> ();
            //Grab the DisplayUnitPluginRepository
			var repoManager = factory.DisplayUnitsPluginRepo;
			//Register each of the plugins into the container
			foreach (var plugin in repoManager.GetAll()) {
                pluginContainer.Register (plugin);
			}
		}

        public void LoadBootstrappers (IBootList bootstrappers)
        {
        }

        public void RegisterDependencies(IRegistrationService rs)
		{
            rs.Register<IDisplayUnitPluginContainer, DisplayUnitPluginContainer> (LifeCycle.Singleton);
            rs.Register<IDisplayUnitPluginFactory, DisplayUnitPluginFactory> (LifeCycle.Transient);
            rs.Register<IDisplayUnitPluginRepoManager, DisplayUnitPluginRepoManager> (LifeCycle.Transient);
		}

	}
}

