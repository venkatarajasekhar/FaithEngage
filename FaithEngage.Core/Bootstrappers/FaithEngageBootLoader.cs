using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.DisplayUnits;
using System.Collections.Generic;
using FaithEngage.Core.Factories;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.Events;
using FaithEngage.Core.Cards;
using FaithEngage.Core.Userclasses;

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
            container.Register<IAppFactory, AppFactory> ();
            container.Register<IRegistrationService, RegistrationService> (LifeCycle.Transient);
		}

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
            var pluginBooter = new PluginBootstrapper ();
            var duBooter = new DisplayUnitBootstrapper ();
            var actionBooter = new ActionProcessorsBootstrapper ();
			var cardProcBooter = new CardProcessorBootstrapper ();
			var cardBooter = new CardBootstrapper();
            var eventBooter = new EventBootstrapper ();
            var userBooter = new UserClassBootstrapper ();

            bootstrappers.Add (pluginBooter);
            bootstrappers.Add (duBooter);
            bootstrappers.Add (actionBooter);
            bootstrappers.Add (cardProcBooter);
            bootstrappers.Add (eventBooter);
			bootstrappers.Add(cardBooter);
            bootstrappers.Add (userBooter);

            pluginBooter.LoadBootstrappers (bootstrappers);
            duBooter.LoadBootstrappers (bootstrappers);
            actionBooter.LoadBootstrappers (bootstrappers);
            cardProcBooter.LoadBootstrappers (bootstrappers);
            eventBooter.LoadBootstrappers (bootstrappers);
			cardBooter.LoadBootstrappers(bootstrappers);
            userBooter.LoadBootstrappers (bootstrappers);
        }
    }
}

