using System;
using System.Collections.Generic;
using FaithEngage.Core.Events;

namespace FaithEngage.Core.RepoInterfaces
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

