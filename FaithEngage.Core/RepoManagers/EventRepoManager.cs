using System;
using System.Collections.Generic;
using FaithEngage.Core.Events;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.RepoInterfaces;
namespace FaithEngage.Core.RepoManagers
{
    public class EventRepoManager : IEventRepoManager
    {
        private readonly IEventRepository _repo;
        public EventRepoManager (IEventRepository repo)
        {
            _repo = repo;
        }
        public void DeleteEvent (Guid id)
        {
			try
			{
				_repo.DeleteEvent(id);
			}
			catch (InvalidIdException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new RepositoryException("There was a problem deleting id " + id + " from the repository.", ex);
			}
        }

        public List<Event> GetByDate (DateTime date, Guid orgId)
        {
            var evnts = _repo.GetByDate (date, orgId);
			if (evnts == null) return new List<Event>();
			return evnts;
        }

        public Event GetById (Guid id)
        {
            return _repo.GetById (id);
        }

        public List<Event> GetByOrgId (Guid orgId)
        {
            return _repo.GetByOrgId (orgId);
        }

        public Guid SaveEvent (Event eventToSave)
        {
            return _repo.SaveEvent (eventToSave);
        }

        public void UpdateEvent (Event eventToUpdate)
        {
            _repo.UpdateEvent (eventToUpdate);
        }
    }
}

