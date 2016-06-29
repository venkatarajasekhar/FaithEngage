using System;

namespace FaithEngage.Core.Events.EventSchedules
{
	public class EventSchedule
	{
		public Guid Id {
			get;
			set;
		}

		public DayOfWeek Day {
			get;
			set;
		}

		private DateTimeOffset _utcStart;
		public TimeSpan UTCStartTime {
			get{
				return _utcStart.TimeOfDay;
			}
		}

		private DateTimeOffset _utcEnd;
		public TimeSpan UTCEndTime {
			get{
				return _utcEnd.TimeOfDay;
			}
		}

		public TimeZoneInfo TimeZone { get; set; }

		public string EventName {
			get;
			set;
		}

		public string EventDescription {
			get;
			set;
		}

		public Guid OrgId{ get; set; }

		public Recurrance Recurrance {
			get;
			set;
		}

		public DateTimeOffset RecurringStart{ get; set; }
		public DateTimeOffset RecurringEnd{ get; set; }

		public void SetUTCStartTime(DateTimeOffset startTime)
		{
			if (TimeZone == null) 
				throw new InvalidTimeZoneException("You cannot set a start time without first setting the TimeZone");
            if(startTime.Offset == new TimeSpan(0)){
                _utcStart = startTime;
            }
            else if (TimeZone?.BaseUtcOffset == startTime.Offset)
			{
                _utcStart = startTime.ToUniversalTime ();
			}
			else {
				throw new InvalidTimeZoneException("The offset for the passed in DateTimeOffset ("
												   + startTime.Offset.TotalHours.ToString()
												   + ") does not match the offset for the TimeZone ("
												   + TimeZone.BaseUtcOffset.TotalHours.ToString());
			}
		}

		public void SetUTCEndTime(DateTimeOffset endTime)
		{
			if (TimeZone == null)
				throw new InvalidTimeZoneException("You cannot set a start time without first setting the TimeZone");
            if(endTime.Offset == new TimeSpan(0))
            {
                _utcEnd = endTime;
            }
            else if (TimeZone?.BaseUtcOffset == endTime.Offset)
			{
                _utcEnd = endTime.ToUniversalTime ();
			}
			else {
				throw new InvalidTimeZoneException("The offset for the passed in DateTimeOffset ("
				                                   + endTime.Offset.TotalHours.ToString()
												   + ") does not match the offset for the TimeZone ("
												   + TimeZone.BaseUtcOffset.TotalHours.ToString());
			}


		}
	}
}

