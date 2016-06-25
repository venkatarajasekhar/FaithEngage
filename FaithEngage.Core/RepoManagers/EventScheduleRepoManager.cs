using System;
using System.Collections.Generic;
using FaithEngage.Core.Events.EventSchedules;
using FaithEngage.Core.Events.EventSchedules.Interfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class EventScheduleRepoManager : IEventScheduleRepoManager
	{
		public void DeleteSchedule(Guid id)
		{
			throw new NotImplementedException();
		}

		public EventSchedule GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public List<EventSchedule> GetByOrgId(Guid id)
		{
			throw new NotImplementedException();
		}

		public Guid SaveSchedule(EventSchedule schedule)
		{
			throw new NotImplementedException();
		}

		public void UpdateSchedule(EventSchedule schedule)
		{
			throw new NotImplementedException();
		}
	}
}

