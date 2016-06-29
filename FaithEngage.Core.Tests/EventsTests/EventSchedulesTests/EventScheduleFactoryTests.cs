using System;
using NUnit.Framework;
namespace FaithEngage.Core.Events.EventSchedules.Factories
{
	[TestFixture]
	public class EventScheduleFactoryTests
	{
		private EventScheduleFactory _fac;
		private Guid VALID_GUID = Guid.NewGuid();

		[SetUp]
		public void Init()
		{
			_fac = new EventScheduleFactory();
		}

		[Test]
		public void Convert_ValidDTO_ValidEventSchedule()
		{
			//Create date times and make variables to test for time conversions.

			var dto = new EventScheduleDTO()
			{
				Day = DayOfWeek.Sunday,
				EventDescription = "TEST",
				EventName = "TEST",
				Id = VALID_GUID,
				OrgId = VALID_GUID,
				Recurrance = Recurrance.Weekly,
				UTCRecurringEnd = DateTime.Parse("12/30/2016"),
				UTCRecurringStart = DateTime.Parse("1/1/2016"),
				TimeZoneId = TimeZoneInfo.Local.Id,
				UTCEndTime = DateTime.Parse("10:00 am").ToUniversalTime().TimeOfDay,
				UTCStartTime = DateTime.Parse("12:00 pm").ToUniversalTime().TimeOfDay
			};

			var evnt = _fac.Convert(dto);

			Assert.That(evnt.Day, Is.EqualTo(DayOfWeek.Sunday));
			Assert.That(evnt.EventName, Is.EqualTo("TEST"));
			Assert.That(evnt.EventDescription, Is.EqualTo("TEST"));
			Assert.That(evnt.Id, Is.EqualTo(VALID_GUID));
			Assert.That(evnt.OrgId, Is.EqualTo(VALID_GUID));
			Assert.That(evnt.Recurrance, Is.EqualTo(Recurrance.Weekly));
			Assert.That(evnt.RecurringEnd, Is.EqualTo(DateTime.Parse("12/30/2016")));
			Assert.That(evnt.RecurringStart, Is.EqualTo(DateTime.Parse("1/1/2016")));
			Assert.That(evnt.TimeZone, Is.EqualTo(TimeZoneInfo.Local));
			Assert.That(evnt.UTCEndTime, Is.EqualTo(dto.UTCEndTime));
			Assert.That(evnt.UTCStartTime, Is.EqualTo(dto.UTCStartTime));
		}
	}
}

