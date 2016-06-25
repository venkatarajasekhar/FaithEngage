using System;
namespace FaithEngage.Core.Events.EventSchedules
{
    public class EventScheduleDTO
    {
        public Guid Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan UTCStartTime { get; set; }
        public TimeSpan UTCEndTime { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public Guid OrgId { get; set; }
        public Recurrance Recurrance { get; set;}
        public DateTime RecurringStart { get; set;}
        public DateTime RecurringEnd { get; set;}
        public TimeZoneInfo TimeZone { get; set;}
	}
}

