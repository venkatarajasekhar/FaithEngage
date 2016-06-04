using System;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.Cards;
using FaithEngage.Facade.Delegates;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using FaithEngage.Core.Events;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Facade
{
	public class FrontEndAccessPoint
	{
		private readonly ICardProcessor _cp;
		private readonly IContainer _container;
		private readonly IAuthenticator _auth;

		public FrontEndAccessPoint (IContainer container)
		{
			_container = container;
			_cp = _container.Resolve<ICardProcessor> ();
			_auth = _container.Resolve<IAuthenticator> ();
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

		public async Task<RenderableCardDTO[]> SignInToLiveEventAsync(Guid eventId, string username)
		{
			var userManager = _container.Resolve<IUserRepoManager> ();
			var eventManager = _container.Resolve<IEventRepoManager> ();
            Event evnt;
            User user;
            try {
                evnt = await Task<Event>.Run(() => eventManager.GetById (eventId));
    			user = await Task<User>.Run(()=> userManager.GetByUsername (username));
            } catch (Exception ex) {
                throw ex;
            }

            if(!_auth.AuthenticateUserToViewEvent(user,evnt)){
				throw new AuthenticationException("User " + username + " is not authorized to view eventId " + eventId.ToString());
			}
			if (OnUserJoinEvent != null) {
                OnUserJoinEvent(new UserEventArgs (){ User = user, Event = evnt });
			}	
            return await Task<RenderableCardDTO>.Run(()=> _cp.GetLiveCardsByEvent (eventId));
		}

		public async Task ExecuteCardActionAsync(string actionName, Dictionary<string,string> parameters, Guid originatingDisplayUnit, string userName)
		{
			var userManager = _container.Resolve<IUserRepoManager> ();
            var user = await Task<User>.Run(()=> userManager.GetByUsername (userName));
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

