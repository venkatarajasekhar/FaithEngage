using System;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.Events.Factories
{
	public class EventFactory : IConverterFactory<EventDTO,Event>
	{
        private readonly IEventScheduleRepoManager _schedRepoMgr;
		public EventFactory(IEventScheduleRepoManager schedRepoMgr)
		{
			_schedRepoMgr = schedRepoMgr;
		}
		public Event Convert(EventDTO dto)
		{
            var evnt = new Event ();
            evnt.AssociatedOrg = dto.AssociatedOrg;
            evnt.EventDate = dto.EventDate;
            evnt.EventId = dto.EventId;
            try {
                evnt.Schedule = _schedRepoMgr.GetById (dto.EventScheduleId);;
            } catch (InvalidIdException) {
                throw new InvalidEventException ("Event schedule id is invalid");
            } catch (Exception){
                throw new RepositoryException ("Unable to access EventSchedule Repository");
            }
            return evnt;
		}
	}
}

