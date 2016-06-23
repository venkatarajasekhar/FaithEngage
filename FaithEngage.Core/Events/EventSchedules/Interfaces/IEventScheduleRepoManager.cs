using System;
using System.Collections.Generic;
using FaithEngage.Core.Events.EventSchedules;

namespace FaithEngage.Core
{
	public interface IEventScheduleRepoManager
	{
		EventSchedule GetById(Guid id);
		List<EventSchedule> GetByOrgId(Guid id);
		Guid SaveSchedule(EventSchedule schedule);
		void UpdateSchedule(EventSchedule schedule);
		void DeleteSchedule(Guid id);
	}
}

