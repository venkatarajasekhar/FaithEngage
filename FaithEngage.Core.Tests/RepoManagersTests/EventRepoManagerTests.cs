using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Events;
using System.Linq;
using System.Collections.Generic;
using FaithEngage.Core.Events.EventSchedules;
using FaithEngage.Core.Tests;

namespace FaithEngage.Core.RepoManagers
{
	[TestFixture]
	public class EventRepoManagerTests
	{
		private IEventRepository _repo;
		private Guid VALID_GUID = Guid.NewGuid();
		private Guid INVALID_GUID = Guid.Empty;
		private const string VALID_STRING = "VALID STRING";
		private EventRepoManager mgr;
        private IConverterFactory<Event, EventDTO> _dtoFac;
        private IConverterFactory<EventDTO, Event> _fac;
		[SetUp]
		public void init()
		{
			_repo = A.Fake<IEventRepository>();
            _dtoFac = A.Fake<IConverterFactory<Event, EventDTO>> ();
            _fac = A.Fake<IConverterFactory<EventDTO, Event>> ();
            mgr = new EventRepoManager(_repo,_fac,_dtoFac);
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
            var dto = new EventDTO () { AssociatedOrg = VALID_GUID, EventDate = DateTime.Now.Date, EventId = VALID_GUID, EventScheduleId = VALID_GUID };

            var dtos = Enumerable.Repeat (dto, 5).ToList ();
            var evnt = new Event ();

            A.CallTo (() => _fac.Convert (A<EventDTO>.Ignored)).Returns (evnt);
            A.CallTo(() => _repo.GetByDate(A<DateTime>.Ignored, A<Guid>.Ignored)).Returns(dtos);

			var evnts = mgr.GetByDate(DateTime.Now.Date, VALID_GUID);
			Assert.That(evnts, Is.Not.Null);
			Assert.That(evnts.Count, Is.EqualTo(5));
            Assert.That(evnts.All(p => p.Equals(evnt)));
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
        public void GetById_ValidId_ReturnsValidEvent ()
        {
            var dto = new EventDTO () { AssociatedOrg = VALID_GUID, EventDate = DateTime.Now.Date, EventId = VALID_GUID, EventScheduleId = VALID_GUID };
            var evnt = new Event();
            A.CallTo(() => _repo.GetById(VALID_GUID)).Returns(dto);
            A.CallTo (() => _fac.Convert (dto)).Returns (evnt);
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
			var dto = new EventDTO () { AssociatedOrg = VALID_GUID, EventDate = DateTime.Now.Date, EventId = VALID_GUID, EventScheduleId = VALID_GUID };
			var dtos = Enumerable.Repeat(dto, 5).ToList();
            var evnt = new Event ();
			A.CallTo(() => _repo.GetByOrgId(VALID_GUID)).Returns(dtos);
            A.CallTo (() => _fac.Convert (A<EventDTO>.Ignored)).Returns (evnt);
			var receivedEvents = mgr.GetByOrgId(VALID_GUID);

            Assert.That(receivedEvents.All(p=> p.Equals(evnt)));
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
		[Test]
		public void SaveEvent_ValidEvent_SavesToRepo()
		{
            var evnt = new Event () {
                AssociatedOrg = VALID_GUID,
                EventDate = DateTime.Now.Date,
                EventId = VALID_GUID,
                Schedule = new EventSchedule () {
                    OrgId = VALID_GUID,
                    Id = VALID_GUID,
                    TimeZone = TimeZoneInfo.Local
				}
			};
            A.CallTo (() => _repo.SaveEvent (A<EventDTO>.Ignored)).Returns (VALID_GUID);

			var id = mgr.SaveEvent(evnt);
            Assert.That (id, Is.EqualTo (VALID_GUID));
            A.CallTo(() => _repo.SaveEvent(A<EventDTO>.Ignored)).MustHaveHappened();

		}

		[Test]
		public void SaveEvent_InvalidEvents_ThrowsInvalidEventExceptions()
		{
			var events = new List<Event>()
			{
				new Event()
				{
					AssociatedOrg = INVALID_GUID,
					Schedule = new EventSchedule()
					{
						OrgId = VALID_GUID,
						Id = VALID_GUID,
						TimeZone = TimeZoneInfo.Local
					}
				},
				new Event()
				{
					AssociatedOrg = VALID_GUID
				},
				new Event()
				{
					AssociatedOrg = VALID_GUID,
					Schedule = new EventSchedule()
					{
						OrgId = INVALID_GUID,
						Id = VALID_GUID,
                        TimeZone = TimeZoneInfo.Local
					}
				},
				new Event()
				{
					AssociatedOrg = VALID_GUID,
					Schedule = new EventSchedule()
					{
						OrgId = VALID_GUID,
						Id = INVALID_GUID,
						TimeZone = TimeZoneInfo.Local
					}
				},
				new Event()
				{
					AssociatedOrg = VALID_GUID,
					Schedule = new EventSchedule()
					{
						OrgId = VALID_GUID,
						Id = VALID_GUID,
					}
				}
			};

			var list = new List<Exception>();
			foreach (var evnt in events)
			{
				var e = TestHelpers.TryGetException(() => mgr.SaveEvent(evnt));
				if (e != null) list.Add(e);
			}

			Assert.That(list.Count, Is.EqualTo(5));
			list.ForEach(p => Assert.That(p, Is.InstanceOf(typeof(InvalidEventException))));
			A.CallTo(()=>_repo.SaveEvent(A<EventDTO>.Ignored)).MustNotHaveHappened();
		}

        [Test]
        public void UpdateEvent_ValidEvent_UpdatesRepo()
        {
            var evnt = new Event () {
                AssociatedOrg = VALID_GUID,
                EventDate = DateTime.Now.Date,
                EventId = VALID_GUID,
                Schedule = new EventSchedule () {
                    OrgId = VALID_GUID,
                    Id = VALID_GUID,
                    TimeZone = TimeZoneInfo.Local
                }
            };
            var dto = new EventDTO ();
            A.CallTo (() => _dtoFac.Convert (evnt)).Returns (dto);

            mgr.UpdateEvent (evnt);

            A.CallTo (() => _repo.UpdateEvent (dto)).MustHaveHappened();
        }

        [Test]
        public void UpdateEvent_InvalidEvents_ThrowsInvalidEventException ()
        {
            var events = new List<Event> ()
            {
                new Event()
                {
                    AssociatedOrg = INVALID_GUID,
                    Schedule = new EventSchedule()
                    {
                        OrgId = VALID_GUID,
                        Id = VALID_GUID,
                        TimeZone = TimeZoneInfo.Local
                    }
                },
                new Event()
                {
                    AssociatedOrg = VALID_GUID
                },
                new Event()
                {
                    AssociatedOrg = VALID_GUID,
                    Schedule = new EventSchedule()
                    {
                        OrgId = INVALID_GUID,
                        Id = VALID_GUID,
                        TimeZone = TimeZoneInfo.Local
                    }
                },
                new Event()
                {
                    AssociatedOrg = VALID_GUID,
                    Schedule = new EventSchedule()
                    {
                        OrgId = VALID_GUID,
                        Id = INVALID_GUID,
                        TimeZone = TimeZoneInfo.Local
                    }
                },
                new Event()
                {
                    AssociatedOrg = VALID_GUID,
                    Schedule = new EventSchedule()
                    {
                        OrgId = VALID_GUID,
                        Id = VALID_GUID,
                    }
                }
            };

            var list = new List<Exception> ();
            foreach (var evnt in events) {
                var e = TestHelpers.TryGetException (() => mgr.UpdateEvent (evnt));
                if (e != null) list.Add (e);
            }

            Assert.That (list.Count, Is.EqualTo (5));
            list.ForEach (p => Assert.That (p, Is.InstanceOf (typeof (InvalidEventException))));
            A.CallTo (() => _repo.SaveEvent (A<EventDTO>.Ignored)).MustNotHaveHappened ();
        }
	}
}

