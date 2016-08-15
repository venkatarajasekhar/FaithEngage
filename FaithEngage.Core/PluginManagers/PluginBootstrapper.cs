using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.PluginManagers.Factories;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.Bootstrappers;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginBootstrapper :IBootstrapper
	{
        public BootPriority BootPriority {
            get {
                return BootPriority.Normal;
            }
        }

        public void Execute(IAppFactory container)
		{
		}

        public void LoadBootstrappers (IBootList bootstrappers)
        {
            bootstrappers.Load<DisplayUnitPluginBootstrapper> ();
            bootstrappers.Load<PluginFileBootstrapper> ();
        }

        public void RegisterDependencies(IRegistrationService rs)
		{
            rs.Register<IPluginManager, PluginManager> (LifeCycle.Singleton);
			rs.Register<IConverterFactory<Plugin, PluginDTO>, PluginDtoFactory>(LifeCycle.Transient);
            rs.Register<IPluginRepoManager, PluginRepoManager> (LifeCycle.Singleton);
		}
	}
}

