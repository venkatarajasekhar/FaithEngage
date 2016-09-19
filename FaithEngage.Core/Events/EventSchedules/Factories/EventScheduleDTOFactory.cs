using System;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events.EventSchedules.Factories
{
    public class EventScheduleDTOFactory : IConverterFactory<EventSchedule,EventScheduleDTO>
	{
		public EventScheduleDTO Convert (EventSchedule evnt)
        {
            var dto = new EventScheduleDTO (); 
            dto.Day = evnt.Day;
            dto.EventDescription = evnt.EventDescription;
            dto.EventName = evnt.EventName;
            dto.Id = evnt.Id;
            dto.OrgId = evnt.Id;
            dto.Recurrance = evnt.Recurrance;
			dto.UTCRecurringEnd = evnt.RecurringEnd.ToUniversalTime().DateTime;
            dto.UTCRecurringStart = evnt.RecurringStart.ToUniversalTime().DateTime;
            dto.UTCEndTime = evnt.UTCEndTime;
            dto.UTCStartTime = evnt.UTCStartTime;
            return dto;
        }
    }
}

