using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.PluginManagers.Factories;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginBootstrapper :IBootstrapper
	{

        public void Execute(IContainer container)
		{
		}

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
            var duBooter = new DisplayUnitPluginBootstrapper ();
            var fileBooter = new PluginFileBootstrapper ();

            bootstrappers.Add (duBooter);
            bootstrappers.Add (fileBooter);

            duBooter.LoadBootstrappers (bootstrappers);
            fileBooter.LoadBootstrappers (bootstrappers);
        }

        public void RegisterDependencies(IContainer container)
		{
            container.Register<IPluginManager, PluginManager> (LifeCycle.Singleton);
			container.Register<IConverterFactory<Plugin, PluginDTO>, PluginDtoFactory>(LifeCycle.Transient);
		}
	}
}

