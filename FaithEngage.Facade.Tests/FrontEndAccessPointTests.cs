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



	}
}

