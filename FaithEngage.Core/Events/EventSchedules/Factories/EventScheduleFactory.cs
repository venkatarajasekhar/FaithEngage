using System;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events.EventSchedules.Factories
{
    /// <summary>
    /// Converts and EventScheduleDTO to an EventSchedule. 
    /// </summary>
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
			//Create the start time offset in UTC by adding a the utc start time to 
			//epoch.
			var startTime = new DateTime(0, DateTimeKind.Utc);
			startTime = startTime.Add(dto.UTCStartTime);
			//Create DateTimeOffset (aware of its offset from UTC)
			var startDtOffset = new DateTimeOffset(startTime);
			//Do the same for end time.
			var endTime = new DateTime(0, DateTimeKind.Utc);
			endTime = endTime.Add(dto.UTCEndTime);
			var endDtOffset = new DateTimeOffset(endTime);
			//Set the utc start and end times from the offsets.
			sched.SetUTCStartTime(startDtOffset);
			sched.SetUTCEndTime(endDtOffset);
			//Set the recurrent start and end times.
			sched.RecurringEnd = new DateTimeOffset(dto.UTCRecurringEnd, new TimeSpan());
			sched.RecurringStart = new DateTimeOffset(dto.UTCRecurringStart, new TimeSpan());

            return sched;
        }
    }
}

