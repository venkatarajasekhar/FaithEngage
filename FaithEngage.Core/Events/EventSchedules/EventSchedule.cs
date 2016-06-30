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
            _utcStart = startTime.ToUniversalTime ();
		}

		public void SetUTCEndTime(DateTimeOffset endTime)
		{
            _utcEnd = endTime.ToUniversalTime ();
		}
	}
}

