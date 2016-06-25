using System;
namespace FaithEngage.Core.Events.EventSchedules.Factories
{
    public class EventScheduleDTOFactory : IConverterFactory<EventSchedule,EventScheduleDTO>
	{
        public EventScheduleDTO Convert (EventSchedule source)
        {
            var dto = new EventScheduleDTO ();
            dto.Day = source.Day;
            dto.EventDescription = source.EventDescription;
            dto.EventName = source.EventName;
            dto.Id = source.Id;
            dto.OrgId = source.Id;
            dto.Recurrance = source.Recurrance;
            dto.RecurringEnd = source.RecurringEnd;
            dto.RecurringStart = source.RecurringStart;
            dto.UTCEndTime = source.UTCEndTime;
            dto.UTCStartTime = source.UTCStartTime;
            dto.TimeZone = source.TimeZone;
            return dto;
        }
    }
}

