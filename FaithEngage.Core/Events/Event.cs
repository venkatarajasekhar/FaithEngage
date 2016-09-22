using System;
using FaithEngage.Core.Events.EventSchedules;

namespace FaithEngage.Core.Events
{
	/// <summary>
	/// Every display unit is associated with an event, and every Event is associated with an Organization
	/// and has an EventSchedule.
	/// </summary>
	public class Event
	{
		/// <summary>
		/// Gets or sets the event identifier.
		/// </summary>
		/// <value>The event identifier.</value>
		public Guid EventId {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the associated organization's id
		/// </summary>
		/// <value>The associated organization's id</value>
		public Guid AssociatedOrg {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the EventSchedule.
		/// </summary>
		/// <value>The schedule.</value>
		public EventSchedule Schedule{ get; set; }
		/// <summary>
		/// Gets or sets the event date.
		/// </summary>
		/// <value>The event date.</value>
		public DateTimeOffset? EventDate{ get; set; }
	}
}

