using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.DisplayUnits;
using System.Collections.Generic;
using FaithEngage.Core.Factories;
using FaithEngage.Core.CardProcessor;

namespace FaithEngage.Core
{
	public class FaithEngageBootLoader : IBootstrapper
	{
        
        public void Execute(IContainer container)
		{
            FEFactory.Activate (container);
		}

		public void RegisterDependencies(IContainer container)
		{
		}

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
            var pluginBooter = new PluginBootstrapper ();
            var duBooter = new DisplayUnitBootstrapper ();
            var actionBooter = new ActionProcessorsBootstrapper ();
            var cardBooter = new CardProcessorBootstrapper ();
            var eventBooter = new EventBootstrapper ();

            bootstrappers.Add (pluginBooter);
            bootstrappers.Add (duBooter);
            bootstrappers.Add (actionBooter);
            bootstrappers.Add (cardBooter);
            bootstrappers.Add (eventBooter);

            pluginBooter.LoadBootstrappers (bootstrappers);
            duBooter.LoadBootstrappers (bootstrappers);
            actionBooter.LoadBootstrappers (bootstrappers);
            cardBooter.LoadBootstrappers (bootstrappers);
            eventBooter.LoadBootstrappers (bootstrappers);
        }
    }
}

