using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.EventSchedules;
using FaithEngage.Core.Events.Factories;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events
{
    public class EventBootstrapper : IBootstrapper
    {

        public void Execute (IAppFactory container)
        {
        }

        public void LoadBootstrappers (IBootList bootstrappers)
        {
            bootstrappers.Load<EventScheduleBootstrapper> ();
        }

        public void RegisterDependencies (IRegistrationService rs)
        {
            rs.Register<IEventRepoManager, EventRepoManager> (LifeCycle.Singleton);
            rs.Register<IConverterFactory<Event, EventDTO>, EventDTOFactory> (LifeCycle.Singleton);
            rs.Register<IConverterFactory<EventDTO, Event>, EventFactory> (LifeCycle.Singleton);
        }
    }
}

