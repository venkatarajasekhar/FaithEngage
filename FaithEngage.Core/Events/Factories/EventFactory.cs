using System;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events.Factories
{
	/// <summary>
	/// Converts an EventDTO to an Event
	/// </summary>
	public class EventFactory : IConverterFactory<EventDTO,Event>
	{
        private readonly IEventScheduleRepoManager _schedRepoMgr;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Events.Factories.EventFactory"/> class.
		/// </summary>
		/// <param name="schedRepoMgr">An IEventScheduleRepoManager</param>
		public EventFactory(IEventScheduleRepoManager schedRepoMgr)
		{
			//Encapsulate the schedule repo manager.
			_schedRepoMgr = schedRepoMgr;
		}
		/// <summary>
		/// Converts an EventDTO to an Event
		/// </summary>
		/// <param name="dto">Dto.</param>
		public Event Convert(EventDTO dto)
		{
            var evnt = new Event ();
            evnt.AssociatedOrg = dto.AssociatedOrg;
			evnt.EventDate = new DateTimeOffset (dto.UtcEventDate.Value, new TimeSpan ());
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

