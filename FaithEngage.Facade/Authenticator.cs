using System;
using System.Linq;
using FaithEngage.Core.Containers;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Events;

namespace FaithEngage.Facade.Interfaces
{
	public class Authenticator : IAuthenticator
	{
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

