using System;
using FaithEngage.Core.Events.EventSchedules;
using NUnit.Framework;
namespace FaithEngage.Core.Events.Factories
{
    [TestFixture]
    public class EventDTOFactoryTests
    {
        private EventDTOFactory _fac;
        private Guid VALID_GUID = Guid.NewGuid ();


        [SetUp]
        public void init()
        {
            _fac = new EventDTOFactory ();
        }

        [Test]
        public void Convert_ValidEvent_ValidDTO()
        {
            var evnt = new Event ();
            evnt.AssociatedOrg = VALID_GUID;
            evnt.EventDate = DateTime.Now.Date;
            evnt.EventId = VALID_GUID;
            evnt.Schedule = new EventSchedule ();
            evnt.Schedule.Id = VALID_GUID;

            var dto = _fac.Convert (evnt);

            Assert.That (dto.AssociatedOrg, Is.EqualTo (VALID_GUID));
            Assert.That (dto.UtcEventDate, Is.EqualTo (DateTime.Now.Date));
            Assert.That (dto.EventId, Is.EqualTo (VALID_GUID));
            Assert.That (dto.EventScheduleId, Is.EqualTo (VALID_GUID));
        }

        [Test]
        public void Convert_InvalidEvent_EmptyDTO()
        {
            var evnt = new Event ();

            var dto = _fac.Convert (evnt);

            Assert.That (dto.AssociatedOrg, Is.EqualTo (Guid.Empty));
            Assert.That (dto.UtcEventDate, Is.Null);
            Assert.That (dto.EventId, Is.EqualTo (Guid.Empty));
            Assert.That (dto.EventScheduleId, Is.EqualTo (Guid.Empty));
        }

    }
}

