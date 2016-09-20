using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Factories;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.Events;
using FaithEngage.Core.Cards;
using FaithEngage.Core.Userclasses;
using FaithEngage.Core.ActionProcessors;

namespace FaithEngage.Core.Bootstrappers
{
    /// <summary>
    /// This is the root bootstrapper for FaithEngage.Core.
    /// </summary>
    public class FaithEngageBootLoader : IBootstrapper
    {
        public BootPriority BootPriority {
            get {
                return BootPriority.First;
            }
        }

        public void Execute (IAppFactory factory)
        {
            //Activate FEFactory to enable static access to the IAppFactory
            FEFactory.Activate (factory);
        }

        public void RegisterDependencies (IRegistrationService rs)
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

