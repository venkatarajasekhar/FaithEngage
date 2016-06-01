using System;

namespace FaithEngage.Core.Events
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

		private DateTime _utcStart;
		public TimeSpan UTCStartTime {
			get{
				return _utcStart.TimeOfDay;
			}
			set{
				_utcStart = new DateTime (value.Ticks).ToUniversalTime ();
			}
		}

		private DateTime _utcEnd;
		public TimeSpan UTCEndTime {
			get{
				return _utcEnd.TimeOfDay;
			}
			set{
				_utcEnd = new DateTime (value.Ticks).ToUniversalTime ();
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

		public DateTime RecurringStart{ get; set; }
		public DateTime RecurringEnd{ get; set; }
	}
}

