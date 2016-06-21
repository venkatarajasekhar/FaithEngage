using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Events;
using System.Linq;

namespace FaithEngage.Core.Tests
{
	[TestFixture]
	public class EventRepoManagerTests
	{
		private IEventRepository _repo;
		private Guid VALID_GUID = Guid.NewGuid();
		private Guid INVALID_GUID = Guid.Empty;
		private const string VALID_STRING = "VALID STRING";
		private EventRepoManager mgr;

		[SetUp]
		public void init()
		{
			_repo = A.Fake<IEventRepository>();
			mgr = new EventRepoManager(_repo);
		}

		[Test]
		public void DeleteEvent_ValidId_DeletesEvent()
		{
			mgr.DeleteEvent(VALID_GUID);
			A.CallTo(() => _repo.DeleteEvent(VALID_GUID)).MustHaveHappened();
		}

		[Test]
		public void DeleteEvent_InvalidId_ThrowsInvalidIdException()
		{
			A.CallTo(() => _repo.DeleteEvent(INVALID_GUID)).Throws<InvalidIdException>();
			var e = TestHelpers.TryGetException(() => mgr.DeleteEvent(INVALID_GUID));
			Assert.That(e, Is.InstanceOf(typeof(InvalidIdException)));
		}

		[Test]
		public void DeleteEvent_RepoThrowsException_ThrowsRepoException()
		{
			A.CallTo(() => _repo.DeleteEvent(VALID_GUID)).Throws<Exception>();
			var e = TestHelpers.TryGetException(() => mgr.DeleteEvent(VALID_GUID));
			Assert.That(e, Is.InstanceOf(typeof(RepositoryException)));
		}

		[Test]
		public void GetByDate_ValidDateAndOrgId_ReturnsValidList()
		{
			var evnt = new Event()
			{
				AssociatedOrg = VALID_GUID,
				EventDate = DateTime.Now.Date,
				EventId = VALID_GUID,
				Schedule = new EventSchedule()
			};
			var events = Enumerable.Repeat(evnt, 5).ToList();
			A.CallTo(() => _repo.GetByDate(A<DateTime>.Ignored, A<Guid>.Ignored)).Returns(events);

			var evnts = mgr.GetByDate(DateTime.Now.Date, VALID_GUID);
			Assert.That(evnts, Is.Not.Null);
			Assert.That(evnts.Count, Is.EqualTo(5));
			Assert.That(evnts.All(p => p.AssociatedOrg == VALID_GUID));
		}

		[Test]
		public void GetByDate_RepoReturnsNull_ReturnsEmptyList()
		{
			A.CallTo(() => _repo.GetByDate(A<DateTime>.Ignored, A<Guid>.Ignored)).Returns(null);
			var evnts = mgr.GetByDate(DateTime.Now.Date, VALID_GUID);

			Assert.That(evnts, Is.Not.Null);
			Assert.That(evnts.Count, Is.EqualTo(0));
		}




	}
}

