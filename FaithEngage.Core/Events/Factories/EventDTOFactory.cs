using System;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events.Factories
{
	public class EventDTOFactory : IConverterFactory<Event,EventDTO>
	{
		public EventDTO Convert(Event evnt)
		{
            var dto = new EventDTO ();
            dto.AssociatedOrg = evnt.AssociatedOrg;
            dto.EventDate = evnt.EventDate;
            dto.EventId = evnt.EventId;
            dto.EventScheduleId = (evnt.Schedule != null) ? evnt.Schedule.Id : Guid.Empty;
            return dto;
		}
	}
}

