﻿using System;
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

		[Test]
		public void GetByDate_InvalidId_ThrowsInvalidIdException()
		{
			A.CallTo(() => _repo.GetByDate(A<DateTime>.Ignored, A<Guid>.Ignored)).Throws<InvalidIdException>();
			var e = TestHelpers.TryGetException(() => mgr.GetByDate(DateTime.Now.Date, INVALID_GUID));

			Assert.That(e, Is.InstanceOf(typeof(InvalidIdException)));
		}

		[Test]
		public void GetByDate_RepoThrowsException_ThrowsRepoException()
		{
			A.CallTo(() => _repo.GetByDate(A<DateTime>.Ignored, A<Guid>.Ignored)).Throws<Exception>();
			var e = TestHelpers.TryGetException(() => mgr.GetByDate(DateTime.Now.Date, INVALID_GUID));

			Assert.That(e, Is.InstanceOf(typeof(RepositoryException)));
		}

		[Test]
		public void GetById_ValidId_ReturnsValidEvent()
		{
			var evnt = new Event()
			{
				AssociatedOrg = VALID_GUID,
				EventDate = DateTime.Now.Date,
				EventId = VALID_GUID,
				Schedule = new EventSchedule()
			};
			A.CallTo(() => _repo.GetById(VALID_GUID)).Returns(evnt);
			var receivedEvent = mgr.GetById(VALID_GUID);

			Assert.That(receivedEvent, Is.EqualTo(evnt));
		}

		[Test]
		public void GetById_InvalidId_ThrowsInvalidIdException()
		{
			A.CallTo(() => _repo.GetById(INVALID_GUID)).Throws<InvalidIdException>();
			var e = TestHelpers.TryGetException(() => mgr.GetById(INVALID_GUID));

			Assert.That(e, Is.InstanceOf(typeof(InvalidIdException)));
		}

		[Test]
		public void GetById_RepoThrowsException_ThrowsRepoException()
		{
			A.CallTo(() => _repo.GetById(INVALID_GUID)).Throws<Exception>();
			var e = TestHelpers.TryGetException(() => mgr.GetById(INVALID_GUID));

			Assert.That(e, Is.InstanceOf(typeof(RepositoryException)));
		}

		[Test]
		public void GetByOrgId_ValidId_ReturnsValidEvent()
		{
			var evnt = new Event()
			{
				AssociatedOrg = VALID_GUID,
				EventDate = DateTime.Now.Date,
				EventId = VALID_GUID,
				Schedule = new EventSchedule()
			};
			var evnts = Enumerable.Repeat(evnt, 5).ToList();

			A.CallTo(() => _repo.GetByOrgId(VALID_GUID)).Returns(evnts);
			var receivedEvents = mgr.GetByOrgId(VALID_GUID);

			Assert.That(receivedEvents, Is.EqualTo(evnts));
		}

		[Test]
		public void GetByOrgId_InvalidId_ThrowsInvalidIdException()
		{
			A.CallTo(() => _repo.GetByOrgId(INVALID_GUID)).Throws<InvalidIdException>();
			var e = TestHelpers.TryGetException(() => mgr.GetByOrgId(INVALID_GUID));

			Assert.That(e, Is.InstanceOf(typeof(InvalidIdException)));
		}

		[Test]
		public void GetByOrgId_RepoThrowsException_ThrowsRepoException()
		{
			A.CallTo(() => _repo.GetByOrgId(INVALID_GUID)).Throws<Exception>();
			var e = TestHelpers.TryGetException(() => mgr.GetByOrgId(INVALID_GUID));

			Assert.That(e, Is.InstanceOf(typeof(RepositoryException)));
		}
	}
}

