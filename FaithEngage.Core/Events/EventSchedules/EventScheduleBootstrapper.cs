using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Events.EventSchedules.Factories;

namespace FaithEngage.Core.Events.EventSchedules
{
	public class EventScheduleBootstrapper : IBootstrapper
	{

        public void Execute(IContainer container)
		{
		}

		public void LoadBootstrappers(IList<IBootstrapper> bootstrappers)
		{
            
		}

		public void RegisterDependencies(IContainer container)
		{
            container.Register<IEventScheduleRepoManager, EventScheduleRepoManager> ();
            container.Register<IConverterFactory<EventSchedule, EventScheduleDTO>, EventScheduleDTOFactory> ();
            container.Register<IConverterFactory<EventScheduleDTO, EventSchedule>, EventScheduleFactory> ();
		}
	}
}

