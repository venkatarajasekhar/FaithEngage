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
		private readonly IEventScheduleRepoManager _schedMgr;
		private readonly IEventFactory _fac;
		private readonly IEventScheduleDTOFactory _dtoFac;
		public EventRepoManager (IEventRepository repo, IEventScheduleRepoManager schedMgr, IEventFactory fac, IEventScheduleDTOFactory dtoFac)
        {
            _repo = repo;
			_schedMgr = schedMgr;
			_fac = fac;
			_dtoFac = dtoFac;
        }
        public void DeleteEvent (Guid id)
        {
			execute(() => _repo.DeleteEvent(id));
        }

        public List<Event> GetByDate (DateTime date, Guid orgId)
        {
			var evnts = execute(() => _repo.GetByDate(date,orgId));
			if (evnts == null) return new List<Event>();
			return evnts;
        }

        public Event GetById (Guid id)
        {
			return execute(()=> _repo.GetById (id));
        }

        public List<Event> GetByOrgId (Guid orgId)
        {
			return execute(()=> _repo.GetByOrgId (orgId));
        }

        public Guid SaveEvent (Event eventToSave)
        {
			if (!validateEvent(eventToSave)) throw new InvalidEventException() { InvalidEvent = eventToSave };
			return execute(()=> _repo.SaveEvent (eventToSave));
        }

        public void UpdateEvent (Event eventToUpdate)
        {
            _repo.UpdateEvent (eventToUpdate);
        }

		private bool validateEvent(Event e)
		{
			try
			{
				check(e.AssociatedOrg, p => p != Guid.Empty);
				check(e.Schedule, p => p != null);
				check(e.Schedule.OrgId, p => p == e.AssociatedOrg);
				check(e.Schedule.Id, p => p != Guid.Empty);
				check(e.Schedule.TimeZone, p => p != null);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		private void check<T>(T val, Func<T, bool> test)
		{
			if (!test(val)) throw new Exception();
		}

		private void execute(Action cmd)
		{
			try
			{
				cmd();
			}
			catch (InvalidIdException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new RepositoryException("There was a problem accessing the repository.", ex);
			}
		}

		private T execute<T>(Func<T> func)
		{
			try
			{
				return func();
			}
			catch (InvalidIdException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new RepositoryException("There was a problem accessing the repository.", ex);
			}
		}

    }
}

