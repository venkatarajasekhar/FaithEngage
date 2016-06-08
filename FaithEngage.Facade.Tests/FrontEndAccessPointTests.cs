using NUnit.Framework;
using System;
using FakeItEasy;
using FaithEngage.Core.Containers;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Cards;
using FaithEngage.Facade.Delegates;
using FaithEngage.Core.Events;
using FaithEngage.Facade.Interfaces;
using FaithEngage.Core.Exceptions;
using System.Collections.Generic;

namespace FaithEngage.Facade.Tests
{
	[TestFixture]
	public class FrontEndAccessPointTests
	{
		private IContainer container;
		private ICardProcessor processor;
		private IAuthenticator auth;
		private IUserRepoManager userRepo;
		private IEventRepoManager eventRepo;
		private Guid VALID_GUID = Guid.NewGuid ();
		private Guid INVALID_GUID = Guid.NewGuid();
		private const string VALID_STRING = "VALID STRING";
		private const string INVALID_STRING = "INVALID STRING";


		[SetUp]
		public void init()
		{
			container = A.Fake<IContainer>();
			processor = A.Fake<ICardProcessor>();
			auth = A.Fake<IAuthenticator>();
			userRepo = A.Fake<IUserRepoManager> ();
			eventRepo = A.Fake<IEventRepoManager> ();
			A.CallTo (() => container.Resolve<ICardProcessor> ()).Returns (processor);
			A.CallTo (() => container.Resolve<IAuthenticator> ()).Returns (auth);
			A.CallTo (() => container.Resolve<IUserRepoManager> ()).Returns (userRepo);
			A.CallTo (() => container.Resolve<IEventRepoManager> ()).Returns (eventRepo);
		}

		[Test]
		public void OnPushNewCard_EventPassesFrom_CardProcessor()
		{
			var feap = new FrontEndAccessPoint (container);
			CardEventArgs ReceivedArgs = null;
			CardEventArgs sentArgs = new CardEventArgs ();
			sentArgs.DisplayUnitId = VALID_GUID;
			sentArgs.EventId = VALID_GUID;
			feap.OnPushNewCard += (CardEventArgs e) => ReceivedArgs = e;

			processor.onPushCard += Raise.With<PushPullEventHandler> (sentArgs);

			Assert.That(ReceivedArgs, Is.EqualTo(sentArgs));
		}

		[Test]
		public void OnPullhCard_EventPassesFrom_CardProcessor()
		{
			var feap = new FrontEndAccessPoint (container);
			CardEventArgs ReceivedArgs = null;
			CardEventArgs sentArgs = new CardEventArgs ();
			sentArgs.DisplayUnitId = VALID_GUID;
			sentArgs.EventId = VALID_GUID;
			feap.OnPullCard += (CardEventArgs e) => ReceivedArgs = e;

			processor.onPullCard += Raise.With<PushPullEventHandler> (sentArgs);

			Assert.That(ReceivedArgs, Is.EqualTo(sentArgs));
		}

		[Test]
		public void OnReRenderCard_EventPassesFrom_CardProcessor()
		{
			var feap = new FrontEndAccessPoint (container);
			CardEventArgs ReceivedArgs = null;
			CardEventArgs sentArgs = new CardEventArgs ();
			sentArgs.DisplayUnitId = VALID_GUID;
			sentArgs.EventId = VALID_GUID;
			feap.OnCardReRender += (CardEventArgs e) => ReceivedArgs = e;

			processor.onReRenderCard += Raise.With<PushPullEventHandler> (sentArgs);

			Assert.That(ReceivedArgs, Is.EqualTo(sentArgs));
		}



		[Test]
		public void SignInToLiveEventAsync_ValidIdAndUsername_Authenticated_Subscribed_ValidArrayOfDTOs ()
		{
			var user = A.Dummy<User> ();
			var evnt = A.Dummy<Event> ();
			UserEventArgs args = null;
			A.CallTo (() => userRepo.GetByUsername (A<string>.Ignored)).Returns (user);
			A.CallTo (() => eventRepo.GetById (A<Guid>.Ignored)).Returns (evnt);
			A.CallTo (() => auth.AuthenticateUserToViewEvent (user, evnt)).Returns (true);

			var feap = new FrontEndAccessPoint (container);
			feap.OnUserJoinEvent += (UserEventArgs e) => args = e;
            var dtos = feap.SignInToLiveEventAsync (VALID_GUID, VALID_STRING).Result;
			A.CallTo (() => processor.GetLiveCardsByEvent (A<Guid>.Ignored)).MustHaveHappened ();
			Assert.That (args, Is.Not.Null);
		}

        [Test]
        public void SignInToLiveEventAsync_ValidIdAndUserName_NotAuthenticated_Subscribed_ThrowsException()
        {
            var user = A.Dummy<User>();
            var evnt = A.Dummy<Event>();
            A.CallTo (() => userRepo.GetByUsername (A<string>.Ignored)).Returns (user);
            A.CallTo (() => eventRepo.GetById (A<Guid>.Ignored)).Returns (evnt);
            A.CallTo (() => auth.AuthenticateUserToViewEvent (user, evnt)).Returns (false);
            UserEventArgs args = null;

            var feap = new FrontEndAccessPoint (container);
            feap.OnUserJoinEvent += (UserEventArgs e) => args = e;
            var task = feap.SignInToLiveEventAsync (VALID_GUID, VALID_STRING);
            Exception exc = null;
            try {
                task.Wait();
            } catch (Exception ex) {
                exc = ex.InnerException;
            }
            Assert.That(exc, Is.InstanceOf(typeof(AuthenticationException)));
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void SignInToLiveEventAsync_EventRepoThrowsException_ValidUserName_Subscribed_ThrowsException()
        {
            var user = A.Dummy<User>();
            var evnt = A.Dummy<Event>();
            A.CallTo (() => userRepo.GetByUsername (A<string>.Ignored)).Returns (user);
            A.CallTo (() => eventRepo.GetById (INVALID_GUID)).Throws<InvalidIdException> ();
            A.CallTo (() => auth.AuthenticateUserToViewEvent (user, evnt)).Returns (false);
            UserEventArgs args = null;

            var feap = new FrontEndAccessPoint (container);
            feap.OnUserJoinEvent += (UserEventArgs e) => args = e;
            var task = feap.SignInToLiveEventAsync (INVALID_GUID, VALID_STRING);
            try {
                task.Wait();
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidUsernameException))]
        public void SignInToLiveEventAsync_UserRepoThrowsException_ValidEventId_Subscribed_ThrowsException()
        {
            var user = A.Dummy<User>();
            var evnt = A.Dummy<Event>();
            A.CallTo (() => userRepo.GetByUsername (INVALID_STRING)).Throws<InvalidUsernameException> ();
            A.CallTo (() => eventRepo.GetById (VALID_GUID)).Returns (evnt);
            A.CallTo (() => auth.AuthenticateUserToViewEvent (user, evnt)).Returns (false);
            UserEventArgs args = null;

            var feap = new FrontEndAccessPoint (container);
            feap.OnUserJoinEvent += (UserEventArgs e) => args = e;
            var task = feap.SignInToLiveEventAsync (VALID_GUID, INVALID_STRING);
            try {
                task.Wait();
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }

		[Test]
		[ExpectedException(typeof(InvalidIdException))]
		public void SignInToLiveEventAsync_CPThrowsException_Throws()
		{
			var user = A.Dummy<User>();
			var evnt = A.Dummy<Event>();
			A.CallTo (() => userRepo.GetByUsername (INVALID_STRING)).Returns (user);
			A.CallTo (() => eventRepo.GetById (VALID_GUID)).Returns (evnt);
			A.CallTo (() => auth.AuthenticateUserToViewEvent (user, evnt)).Returns (true);
			A.CallTo (() => processor.GetLiveCardsByEvent (A<Guid>.Ignored)).Throws<InvalidIdException> ();
			UserEventArgs args = null;

			var feap = new FrontEndAccessPoint (container);
			feap.OnUserJoinEvent += (UserEventArgs e) => args = e;
			var task = feap.SignInToLiveEventAsync (VALID_GUID, INVALID_STRING);
			try {
				task.Wait();
			} catch (Exception ex) {
				throw ex.InnerException;
			}
		}

		[Test]
		public void ExecuteCardActionAsync_ValidParams_ExecutesCorrectly()
		{
			var actionName = VALID_STRING;
			var paramsDict = new Dictionary<string,string> () {
				{ "first", "First String" },
				{ "second", "Second String" }
			};
			var duId = VALID_GUID;
			var userName = VALID_STRING;
			var user = A.Dummy<User> ();
			CardAction action = null;

			var feap = new FrontEndAccessPoint (container);
			A.CallTo (() => userRepo.GetByUsername (VALID_STRING)).Returns (user);
			A.CallTo (() => processor.ExecuteCardActionAsync (A<CardAction>.Ignored))
				.Invokes ((CardAction a) => action = a);

			var task = feap.ExecuteCardActionAsync (actionName, paramsDict, duId, userName);
			task.Wait ();
			A.CallTo (() => processor.ExecuteCardActionAsync (A<CardAction>.Ignored)).MustHaveHappened ();
			Assert.That (action, Is.Not.Null);
			Assert.That (action.ActionName, Is.EqualTo (actionName));
			Assert.That (action.Parameters, Is.EqualTo (paramsDict));
			Assert.That (action.OriginatingDisplayUnit, Is.EqualTo (duId));
			Assert.That (action.User, Is.EqualTo (user));
			Assert.That (task.Exception, Is.Null);
		}

		[Test]
		public void ExecuteCardActionAsync_userRepoThrowsException_Throws ()
		{
			A.CallTo (() => userRepo.GetByUsername (A<string>.Ignored)).Throws<Exception> ();
			var actionName = VALID_STRING;
			var paramsDict = new Dictionary<string,string> () {
				{ "first", "First String" },
				{ "second", "Second String" }
			};
			var duId = VALID_GUID;
			var userName = VALID_STRING;
			var user = A.Dummy<User> ();
			var feap = new FrontEndAccessPoint (container);
			var task = feap.ExecuteCardActionAsync (actionName, paramsDict, duId, userName);
			A.CallTo (() => processor.ExecuteCardActionAsync (A<CardAction>.Ignored)).MustNotHaveHappened ();
			Assert.That (task.Exception, Is.Not.Null);
		}

	}
}

