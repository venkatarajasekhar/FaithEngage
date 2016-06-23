using System;
using System.Collections.Generic;
using FaithEngage.Core.Events;

namespace FaithEngage.Core.RepoInterfaces
{
	public interface IEventRepository
	{
		EventDTO GetById(Guid id);
		List<EventDTO> GetByDate (DateTime date, Guid orgId);
		List<EventDTO> GetByOrgId (Guid orgId);
		Guid SaveEvent(EventDTO eventToSave);
		void UpdateEvent(EventDTO eventToUpdate);
		void DeleteEvent(Guid id);
	}
}

