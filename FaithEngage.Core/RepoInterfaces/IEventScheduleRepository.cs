using System;
using System.Collections.Generic;
using FaithEngage.Core.Events.EventSchedules;

namespace FaithEngage.Core.RepoInterfaces
{
	public interface IEventScheduleRepository
	{
		EventScheduleDTO GetById(Guid id);
		List<EventScheduleDTO> GetByOrgId(Guid id);
		Guid SaveSchedule(EventScheduleDTO schedule);
		void UpdateSchedule(EventScheduleDTO schedule);
		void DeleteSchedule(Guid id);
	}
}

