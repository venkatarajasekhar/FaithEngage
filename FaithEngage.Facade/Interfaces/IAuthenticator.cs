using System;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Events;

namespace FaithEngage.Facade
{
	public interface IAuthenticator
	{
		bool AuthenticateUserToViewEvent (User user, Event evnt);
	}
}

