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

namespace FaithEngage.Core.Bootstrappers
{
	public class FaithEngageBootLoader : IBootstrapper
	{
        public BootPriority BootPriority {
            get {
                return BootPriority.First;
            }
        }

        public void Execute(IAppFactory factory)
		{
            FEFactory.Activate (factory);
		}

        public void RegisterDependencies(IRegistrationService rs)
		{
            rs.Register<IAppFactory, AppFactory> (LifeCycle.Singleton);
		}

        public void LoadBootstrappers (IBootList bootstrappers)
        {
            bootstrappers.Load<PluginBootstrapper> ();
            bootstrappers.Load<DisplayUnitBootstrapper> ();
            bootstrappers.Load<ActionProcessorsBootstrapper> ();
            bootstrappers.Load<CardProcessorBootstrapper> ();
            bootstrappers.Load<CardBootstrapper> ();
            bootstrappers.Load<EventBootstrapper> ();
            bootstrappers.Load<UserClassBootstrapper> ();
        }
    }
}

