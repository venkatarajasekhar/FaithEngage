using System;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events.Factories
{
	/// <summary>
	/// Converts an Event to an EventDTO
	/// </summary>
	public class EventDTOFactory : IConverterFactory<Event,EventDTO>
	{
		/// <summary>
		/// Converts an Event to an EventDTO
		/// </summary>
		/// <param name="evnt">Evnt.</param>
		public EventDTO Convert(Event evnt)
		{
            var dto = new EventDTO ();
            dto.AssociatedOrg = evnt.AssociatedOrg;
			dto.UtcEventDate = evnt.EventDate.Value.UtcDateTime;
            dto.EventId = evnt.EventId;
            dto.EventScheduleId = (evnt.Schedule != null) ? evnt.Schedule.Id : Guid.Empty;
            return dto;
		}
	}
}

