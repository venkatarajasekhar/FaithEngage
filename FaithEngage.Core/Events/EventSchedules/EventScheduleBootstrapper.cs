using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Events.EventSchedules.Factories;
using FaithEngage.Core.Factories;
using FaithEngage.Core.Bootstrappers;

namespace FaithEngage.Core.Events.EventSchedules
{
	public class EventScheduleBootstrapper : IBootstrapper
	{
        public BootPriority BootPriority {
            get {
                return BootPriority.Normal;
            }
        }

        public void Execute(IAppFactory container)
		{
		}

        public void LoadBootstrappers(IBootList bootstrappers)
		{
            
		}

        public void RegisterDependencies(IRegistrationService rs)
		{
            rs.Register<IEventScheduleRepoManager, EventScheduleRepoManager> ();
            rs.Register<IConverterFactory<EventSchedule, EventScheduleDTO>, EventScheduleDTOFactory> ();
            rs.Register<IConverterFactory<EventScheduleDTO, EventSchedule>, EventScheduleFactory> ();
		}
	}
}

