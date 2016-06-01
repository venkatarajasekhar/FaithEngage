using System;
using System.Linq;
using FaithEngage.Core.Containers;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Events;

namespace FaithEngage.Facade
{
	public class Authenticator
	{
		private readonly IContainer _container;
		public Authenticator (IContainer container)
		{
			_container = container;
		}

		public bool AuthenticateUserToViewEvent(User user, Event evnt)
		{
			return orgCheck(user, evnt);
		}

		private bool orgCheck(User user, Event evnt)
		{
			return user.AssignedOrganizations.Any (p => p.Key == evnt.AssociatedOrg);
		}
	}
}

