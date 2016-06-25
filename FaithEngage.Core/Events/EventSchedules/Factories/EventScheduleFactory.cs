using System;
namespace FaithEngage.Core.Events.EventSchedules.Factories
{
    public class EventScheduleFactory : IConverterFactory<EventScheduleDTO,EventSchedule>
	{

        public EventSchedule Convert (EventScheduleDTO source)
        {
            var sched = new EventSchedule ();
            sched.Day = source.Day;
            sched.EventDescription = source.EventDescription;
            sched.EventName = source.EventName;
            sched.Id = source.Id;
            sched.OrgId = source.OrgId;
            sched.Recurrance = source.Recurrance;
            sched.RecurringEnd = source.RecurringEnd;
            sched.RecurringStart = source.RecurringStart;
            sched.TimeZone = source.TimeZone;
            sched.UTCEndTime = source.UTCEndTime;
            sched.UTCStartTime = source.UTCStartTime;
            return sched;
        }
    }
}

