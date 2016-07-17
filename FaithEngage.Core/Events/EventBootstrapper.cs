using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.EventSchedules;
using FaithEngage.Core.Events.Factories;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core.Events
{
    public class EventBootstrapper : IBootstrapper
    {

        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
            var schedBooter = new EventScheduleBootstrapper ();

            bootstrappers.Add (schedBooter);

            schedBooter.LoadBootstrappers (bootstrappers);
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<IEventRepoManager, EventRepoManager> (LifeCycle.Singleton);
            container.Register<IConverterFactory<Event, EventDTO>, EventDTOFactory> (LifeCycle.Singleton);
            container.Register<IConverterFactory<EventDTO, Event>, EventFactory> (LifeCycle.Singleton);
        }
    }
}

