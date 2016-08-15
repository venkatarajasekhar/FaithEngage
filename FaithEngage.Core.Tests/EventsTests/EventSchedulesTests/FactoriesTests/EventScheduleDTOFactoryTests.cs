using System;
using NUnit.Framework;
namespace FaithEngage.Core.Events.EventSchedules.Factories
{
	[TestFixture]
	public class EventScheduleDTOFactoryTests
	{
		private EventScheduleDTOFactory _fac;
		private Guid VALID_GUID = Guid.NewGuid();
		private DateTimeOffset startTime = new DateTimeOffset(DateTime.Parse("10:00am"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset endTime = new DateTimeOffset(DateTime.Parse("12:00pm"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset recurringStart = new DateTimeOffset(DateTime.Parse("12/30/2016"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset recurringEnd = new DateTimeOffset(DateTime.Parse("1/1/2016"), TimeZoneInfo.Local.BaseUtcOffset);

		[SetUp]
		public void Init()
		{
			_fac = new EventScheduleDTOFactory();
		}

		[Test]
		public void Convert_ValidEventSchedule_ValidDTO()
		{
			var sched = new EventSchedule()
			{
				Day = DayOfWeek.Monday,
				EventDescription = "TEST",
				EventName = "TEST",
				Id = VALID_GUID,
				OrgId = VALID_GUID,
				Recurrance = Recurrance.Weekly,
				RecurringEnd = recurringEnd,
				RecurringStart = recurringStart
			};
			sched.SetUTCEndTime(endTime);
			sched.SetUTCStartTime(startTime);

			var dto = _fac.Convert(sched);

			Assert.That(dto.Day, Is.EqualTo(DayOfWeek.Monday));
			Assert.That(dto.EventDescription, Is.EqualTo("TEST"));
			Assert.That(dto.EventName, Is.EqualTo("TEST"));
			Assert.That(dto.Id, Is.EqualTo(VALID_GUID));
			Assert.That(dto.OrgId, Is.EqualTo(VALID_GUID));
			Assert.That(dto.Recurrance, Is.EqualTo(Recurrance.Weekly));
			Assert.That(dto.UTCEndTime, Is.EqualTo(endTime.UtcDateTime.TimeOfDay));
			Assert.That(dto.UTCStartTime, Is.EqualTo(startTime.UtcDateTime.TimeOfDay));
			Assert.That(dto.UTCRecurringEnd, Is.EqualTo(recurringEnd.UtcDateTime));
			Assert.That(dto.UTCRecurringStart, Is.EqualTo(recurringStart.UtcDateTime));
		}

		[Test]
		public void Convert_InvalidEventSchedule_EmptyDTO()
		{
			var sched = new EventSchedule();

			var dto = _fac.Convert(sched);

			Assert.That(dto, Is.Not.Null);
		}
	}
}

