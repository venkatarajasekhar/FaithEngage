using System;
using FaithEngage.Core.Events.EventSchedules;

namespace FaithEngage.Core.Events
{
	public class Event
	{
		public Guid EventId {
			get;
			set;
		}

		public Guid AssociatedOrg {
			get;
			set;
		}

		public EventSchedule Schedule{ get; set; }

		public DateTime EventDate{ get; set; }
	}
}

