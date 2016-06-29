using System;
namespace FaithEngage.Core.Events.EventSchedules.Factories
{
    public class EventScheduleFactory : IConverterFactory<EventScheduleDTO,EventSchedule>
	{
		
        public EventSchedule Convert (EventScheduleDTO dto)
        {
            var sched = new EventSchedule ();
            sched.Day = dto.Day;
            sched.EventDescription = dto.EventDescription;
            sched.EventName = dto.EventName;
            sched.Id = dto.Id;
            sched.OrgId = dto.OrgId;
            sched.Recurrance = dto.Recurrance;
            sched.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(dto.TimeZoneId);
			sched.RecurringEnd = new DateTimeOffset(dto.UTCRecurringStart);
			sched.RecurringStart = new DateTimeOffset(dto.UTCRecurringStart);

			var startTime = new DateTime(1970, 1, 1);
			startTime.Add(dto.UTCStartTime);
			var startDtOffset = new DateTimeOffset(startTime);

			var endTime = new DateTime(1970, 1, 1);
			endTime.Add(dto.UTCEndTime);
			var endDtOffset = new DateTimeOffset(endTime);

			sched.SetUTCEndTime(startDtOffset);
			sched.SetUTCEndTime(endDtOffset);
            return sched;
        }
    }
}

