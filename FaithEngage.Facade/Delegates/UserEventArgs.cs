using System;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Events;

namespace FaithEngage.Facade.Delegates
{
	public class UserEventArgs
	{
		public User User {
			get;
			set;
		}

		public Event Event{ get; set; }
	}
}

