using System;
using System.Collections.Generic;
using FaithEngage.Core.Events;
using FaithEngage.Core.Events.EventSchedules;

namespace FaithEngage.Core.UserClasses
{
	public class Organization
	{
		public Guid OrgId {
			get;
			set;
		}

		public string OrgName {
			get;
			set;
		}

		public string Address{ get; set; }

		public string City{ get; set; }

		public string PhoneNumber { get; set; }

		public string PrimaryAdminUser { get; set; }

		public List<EventSchedule> Events {
			get;
			set;
		}

		public TimeZoneInfo DefaultEventTimeZone { get; set; }
	}
}

