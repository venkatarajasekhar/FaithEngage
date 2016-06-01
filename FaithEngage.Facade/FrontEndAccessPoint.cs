using System;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.Cards;
using FaithEngage.Facade.Delegates;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Exceptions;
using System.Collections.Generic;

namespace FaithEngage.Facade
{
	public class FrontEndAccessPoint
	{
		private readonly CardProcessor _cp;
		private readonly IContainer _container;
		private readonly Authenticator _auth;

		public FrontEndAccessPoint (IContainer container)
		{
			_container = container;
			_cp = _container.Resolve<CardProcessor> ();
			_auth = _container.Resolve<Authenticator> ();
		}

		public event PushPullEventHandler OnPushNewCard {
			add{ _cp.onPushCard += value;}
			remove{ _cp.onPushCard -= value;}
		}

		public event PushPullEventHandler OnPullCard {
			add{ _cp.onPullCard += value;}
			remove{ _cp.onPullCard -= value;}
		}

		public event PushPullEventHandler OnCardReRender{
			add{ _cp.onReRenderCard += value;}
			remove{ _cp.onReRenderCard -= value;}
		}

		public event UserEventHandler OnUserJoinEvent;

		public RenderableCardDTO[] SignInToLiveEvent(Guid eventId, string username)
		{
			var userManager = _container.Resolve<IUserRepoManager> ();
			var eventManager = _container.Resolve<IEventRepoManager> ();
			var evnt = eventManager.GetById (eventId);
			var user = userManager.GetByUsername (username);
			if(!_auth.AuthenticateUserToViewEvent(user,evnt)){
				throw new AuthenticationException("User " + username + " is not authorized to view eventId " + eventId.ToString());
			}
			if (OnUserJoinEvent != null) {
			}
				OnUserJoinEvent(new UserEventArgs (){ User = user, Event = evnt });
			return _cp.GetLiveCardsByEvent (eventId);
		}

		public void ExecuteCardAction(string actionName, Dictionary<string,string> parameters, Guid originatingDisplayUnit, string userName)
		{
			var userManager = _container.Resolve<IUserRepoManager> ();
			var user = userManager.GetByUsername (userName);
			var action = new CardAction () {
				ActionName = actionName,
				Parameters = parameters,
				OriginatingDisplayUnit = originatingDisplayUnit,
				User = user
			};	
			_cp.ExecuteCardAction (action);
		}
	}
}

