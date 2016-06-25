using System;
using FaithEngage.Core.Events.Interfaces;

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
            dto.EventScheduleId = evnt.Schedule.Id;
            return dto;
		}
	}
}

