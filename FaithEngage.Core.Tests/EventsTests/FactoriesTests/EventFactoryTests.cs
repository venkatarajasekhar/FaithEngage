using System;
using NUnit.Framework;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FakeItEasy;
using FaithEngage.Core.Events.EventSchedules;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Tests;

namespace FaithEngage.Core.Events.Factories
{
    [TestFixture]
    public class EventFactoryTests
    {
        private IEventScheduleRepoManager _schedMgr;
        private EventFactory _fac;
        private Guid VALID_GUID = Guid.NewGuid ();

        [SetUp]
        public void Init ()
        {
            _schedMgr = A.Fake<IEventScheduleRepoManager> ();
            _fac = new EventFactory (_schedMgr);
        }

        [Test]
        public void Convert_ValidDTO_CreatesValidEvent ()
        {
            var dto = new EventDTO ();
            dto.AssociatedOrg = VALID_GUID;
			dto.UtcEventDate = DateTime.UtcNow.Date;
            dto.EventId = VALID_GUID;
            dto.EventScheduleId = VALID_GUID;
            var sched = new EventSchedule ();
            A.CallTo (() => _schedMgr.GetById (VALID_GUID)).Returns (sched);

            var evnt = _fac.Convert (dto);

            Assert.That (evnt.AssociatedOrg, Is.EqualTo (VALID_GUID));
			Assert.That (evnt.EventDate.Value.Date, Is.EqualTo (DateTimeOffset.Now.Date));
            Assert.That (evnt.EventId, Is.EqualTo (VALID_GUID));
            Assert.That (evnt.Schedule, Is.EqualTo (sched));
        }

		[Test]
		public void Convert_DTOWithInvalidSchedId_ThrowsInvalidEventException()
		{
			var dto = new EventDTO();

			A.CallTo(() => _schedMgr.GetById(A<Guid>.Ignored)).Throws<InvalidIdException>();

			var e = TestHelpers.TryGetException(() => _fac.Convert(dto));

			Assert.That(e, Is.InstanceOf(typeof(InvalidEventException)));
		}

		[Test]
		public void Convert_SchedRepoThrowsException_ThrowsRepoException()
		{
			var dto = new EventDTO();

			A.CallTo(() => _schedMgr.GetById(A<Guid>.Ignored)).Throws<Exception>();

			var e = TestHelpers.TryGetException(() => _fac.Convert(dto));

			Assert.That(e, Is.InstanceOf(typeof(RepositoryException)));
		}

    }
}

