using System;
using System.Collections.Generic;
using FaithEngage.Core.Events;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.RepoInterfaces;
using System.Linq;
namespace FaithEngage.Core.RepoManagers
{
	public class EventRepoManager : IEventRepoManager
    {
        private readonly IEventRepository _repo;
		private readonly IConverterFactory<EventDTO,Event> _fac;
		private readonly IConverterFactory<Event,EventDTO> _dtoFac;
		public EventRepoManager (IEventRepository repo, IConverterFactory<EventDTO, Event> fac, IConverterFactory<Event, EventDTO> dtoFac)
        {
            _repo = repo;
			_fac = fac;
			_dtoFac = dtoFac;
        }
        public void DeleteEvent (Guid id)
        {
			execute(() => _repo.DeleteEvent(id));
        }

		public List<Event> GetByDate(DateTime date, Guid orgId)
		{
			var dtos = execute(() => _repo.GetByDate(date, orgId));
			if (dtos == null) return new List<Event>();
			return getList<EventDTO, Event>(dtos, _fac);
        }

        public Event GetById (Guid id)
        {
			var dto = execute(()=> _repo.GetById (id));
			return _fac.Convert(dto);
        }

        public List<Event> GetByOrgId (Guid orgId)
        {
			var dtos = execute(()=> _repo.GetByOrgId (orgId));
			if (dtos == null) return new List<Event>();
			return getList<EventDTO, Event>(dtos, _fac);
        }

		private List<Tout> getList<Tin,Tout>(List<Tin> dtos, IConverterFactory<Tin,Tout> fac) where Tout: class
		{
			var events = dtos.Select(p =>
			{
				try
				{
					return fac.Convert(p);
				}
				catch (Exception)
				{
					return null;
				}
			}).Where(p => p != null).ToList();
			return events;
		}

        public Guid SaveEvent (Event eventToSave)
        {
            if (!validateEvent(eventToSave)) throw new InvalidEventException() { InvalidEvent = eventToSave };
            var dto = _dtoFac.Convert (eventToSave);
			return execute(()=> _repo.SaveEvent (dto));
        }

        public void UpdateEvent (Event eventToUpdate)
        {
            if (!validateEvent (eventToUpdate)) throw new InvalidEventException () { InvalidEvent = eventToUpdate};
            var dto = _dtoFac.Convert (eventToUpdate);
            _repo.UpdateEvent (dto);
        }

		private bool validateEvent(Event e)
		{
			try
			{
				check(e.AssociatedOrg, p => p != Guid.Empty);
				check(e.Schedule, p => p != null);
				check(e.Schedule.OrgId, p => p == e.AssociatedOrg);
				check(e.Schedule.Id, p => p != Guid.Empty);
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

