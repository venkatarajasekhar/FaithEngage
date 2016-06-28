using System;
using NUnit.Framework;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FakeItEasy;
using FaithEngage.Core.Events.EventSchedules;

namespace FaithEngage.Core.Events.Factories
{
    [TestFixture]
    public class EventFactoryTests
    {
        private IEventScheduleRepoManager _schedMgr;
        private EventFactory _fac;
        private Guid VALID_GUID = Guid.NewGuid ();
        private Guid INVALID_GUID = Guid.Empty;

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
            dto.EventDate = DateTime.Now.Date;
            dto.EventId = VALID_GUID;
            dto.EventScheduleId = VALID_GUID;
            var sched = new EventSchedule ();
            A.CallTo (() => _schedMgr.GetById (VALID_GUID)).Returns (sched);

            var evnt = _fac.Convert (dto);

            Assert.That (evnt.AssociatedOrg, Is.EqualTo (VALID_GUID));
            Assert.That (evnt.EventDate, Is.EqualTo (DateTime.Now.Date));
            Assert.That (evnt.EventId, Is.EqualTo (VALID_GUID));
            Assert.That (evnt.Schedule, Is.EqualTo (sched));
        }

    }
}

