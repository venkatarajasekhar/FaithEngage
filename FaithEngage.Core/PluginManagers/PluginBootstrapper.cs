using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginBootstrapper :IBootstrapper
	{
        

        public void Execute(IContainer container)
		{
		}

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
            IBootstrapper duBooter = new DisplayUnitPluginBootstrapper ();
            bootstrappers.Add (duBooter);
            duBooter.LoadBootstrappers (bootstrappers);
        }

        public void RegisterDependencies(IContainer container)
		{
		}
	}
}

