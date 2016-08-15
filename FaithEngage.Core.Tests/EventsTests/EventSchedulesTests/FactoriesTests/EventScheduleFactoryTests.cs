using System;
using NUnit.Framework;
namespace FaithEngage.Core.Events.EventSchedules.Factories
{
	[TestFixture]
	public class EventScheduleFactoryTests
	{
		private EventScheduleFactory _fac;
		private Guid VALID_GUID = Guid.NewGuid();
		private DateTimeOffset startTime = new DateTimeOffset(DateTime.Parse("10:00am"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset endTime = new DateTimeOffset(DateTime.Parse("12:00pm"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset recurringStart = new DateTimeOffset(DateTime.Parse ("12/30/2016"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset recurringEnd = new DateTimeOffset(DateTime.Parse("1/1/2016"), TimeZoneInfo.Local.BaseUtcOffset);

		[SetUp]
		public void Init()
		{
			_fac = new EventScheduleFactory();
		}

		[Test]
		public void Convert_ValidDTO_ValidEventSchedule()
		{  
            var dto = new EventScheduleDTO () {
				Day = DayOfWeek.Monday,
                EventDescription = "TEST",
                EventName = "TEST",
                Id = VALID_GUID,
                OrgId = VALID_GUID,
                Recurrance = Recurrance.Weekly,
				UTCRecurringEnd = recurringEnd.UtcDateTime,
				UTCRecurringStart = recurringStart.UtcDateTime,
                UTCEndTime = startTime.UtcDateTime.TimeOfDay,
                UTCStartTime = endTime.UtcDateTime.TimeOfDay,
			};

			var evnt = _fac.Convert(dto);

			Assert.That(evnt.Day, Is.EqualTo(DayOfWeek.Monday));
			Assert.That(evnt.EventName, Is.EqualTo("TEST"));
			Assert.That(evnt.EventDescription, Is.EqualTo("TEST"));
			Assert.That(evnt.Id, Is.EqualTo(VALID_GUID));
			Assert.That(evnt.OrgId, Is.EqualTo(VALID_GUID));
			Assert.That(evnt.Recurrance, Is.EqualTo(Recurrance.Weekly));
            Assert.That(evnt.RecurringEnd.ToUniversalTime(), Is.EqualTo(recurringEnd.ToUniversalTime()));
            Assert.That(evnt.RecurringStart.ToUniversalTime(), Is.EqualTo(recurringStart.ToUniversalTime()));
			Assert.That(evnt.UTCEndTime, Is.EqualTo(startTime.UtcDateTime.TimeOfDay));
			Assert.That(evnt.UTCStartTime, Is.EqualTo(endTime.UtcDateTime.TimeOfDay));
		}

		[Test]
		public void Convert_EmptyDTO_EmptyEventSchedule()
		{
			var dto = new EventScheduleDTO();

			var evnt = _fac.Convert(dto);

			Assert.That(evnt, Is.Not.Null);
		}
	}
}

