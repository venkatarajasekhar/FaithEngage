using System;

namespace FaithEngage.Core.Events.EventSchedules
{
	/// <summary>
	/// Every Event has an EventSchedule. It determines when it occurs, if it is recurring,
	/// when it starts and when it ends.
	/// </summary>
	public class EventSchedule
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public Guid Id {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the day of the week the event will occur on.
		/// </summary>
		/// <value>The day.</value>
		public DayOfWeek Day {
			get;
			set;
		}
		//Start time is saved internally as a DateTimeOffset, but accessed as a TimeSpan
		//from 0 in UTC.
		private DateTimeOffset _utcStart;
		/// <summary>
		/// Gets the UTC start time.
		/// </summary>
		/// <value>The UTC start time.</value>
		public TimeSpan UTCStartTime {
			get{
				return _utcStart.UtcDateTime.TimeOfDay;
			}
		}
		//End time is saved internally as a DateTimeOffset.
		private DateTimeOffset _utcEnd;
		public TimeSpan UTCEndTime {
			get{
				return _utcEnd.UtcDateTime.TimeOfDay;
			}
		}

		/// <summary>
		/// Gets or sets the name of the event.
		/// </summary>
		/// <value>The name of the event.</value>
		public string EventName {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the event description.
		/// </summary>
		/// <value>The event description.</value>
		public string EventDescription {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the org identifier.
		/// </summary>
		/// <value>The org identifier.</value>
		public Guid OrgId{ get; set; }

		/// <summary>
		/// Gets or sets the recurrance.
		/// </summary>
		/// <value>The recurrance.</value>
		public Recurrance Recurrance {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets DateTimeOffset the recurrance will begin.
		/// </summary>
		/// <value>The recurring start.</value>
		public DateTimeOffset RecurringStart{ get; set; }
		/// <summary>
		/// Gets or sets the DateTimeOffset the recurrance will end.
		/// </summary>
		/// <value>The recurring end.</value>
		public DateTimeOffset RecurringEnd{ get; set; }

		/// <summary>
		/// Sets the start time, converted to UTC.
		/// </summary>
		/// <param name="startTime">Start time.</param>
		public void SetStartTime(DateTimeOffset startTime)
		{
            _utcStart = startTime.ToUniversalTime ();
		}

		/// <summary>
		/// Sets the UTC end time, converted to UTC
		/// </summary>
		/// <param name="endTime">End time.</param>
		public void SetEndTime(DateTimeOffset endTime)
		{
            _utcEnd = endTime.ToUniversalTime ();
		}
	}
}

