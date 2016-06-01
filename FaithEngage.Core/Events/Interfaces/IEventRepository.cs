using System;
using System.Collections.Generic;

namespace FaithEngage.Core.Events.Interfaces
{
	public interface IEventRepository
	{
		Event GetById(Guid id);
		List<Event> GetByDate (DateTime date, Guid orgId);
		List<Event> GetByOrgId (Guid orgId);
		Guid SaveEvent(Event eventToSave);
		void UpdateEvent(Event eventToUpdate);
		void DeleteEvent(Guid id);
	}
}

