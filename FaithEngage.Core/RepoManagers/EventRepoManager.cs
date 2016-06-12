using System;
using System.Collections.Generic;
using FaithEngage.Core.Events;
using FaithEngage.Core.Events.Interfaces;
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
            _repo.DeleteEvent (id);
        }

        public List<Event> GetByDate (DateTime date, Guid orgId)
        {
            return _repo.GetByDate (date, orgId);
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

