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

namespace FaithEngage.Facade.Tests
{
	[TestFixture ()]
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

		[Test ()]
		public void SignInToLiveEvent_ValidIdAndUsername_Authenticated_Subscribed_ValidArrayOfDTOs ()
		{
			var user = A.Dummy<User> ();
			var evnt = A.Dummy<Event> ();
			UserEventArgs args = null;
			A.CallTo (() => userRepo.GetByUsername (A<string>.Ignored)).Returns (user);
			A.CallTo (() => eventRepo.GetById (A<Guid>.Ignored)).Returns (evnt);
			A.CallTo (() => auth.AuthenticateUserToViewEvent (user, evnt)).Returns (true);

			var feap = new FrontEndAccessPoint (container);
			feap.OnUserJoinEvent += (UserEventArgs e) => args = e;
			feap.SignInToLiveEvent (VALID_GUID, VALID_STRING);

			A.CallTo (() => processor.GetLiveCardsByEvent (A<Guid>.Ignored)).MustHaveHappened ();
			Assert.That (args, Is.Not.Null);
		}
	}
}

