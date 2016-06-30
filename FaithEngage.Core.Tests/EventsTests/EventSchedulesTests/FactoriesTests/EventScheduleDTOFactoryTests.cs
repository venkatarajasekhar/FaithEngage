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
				Recurrance = Recurrance.Daily,
				RecurringEnd = recurringEnd,
				RecurringStart = recurringStart
			};
			sched.SetUTCEndTime(endTime);
			sched.SetUTCStartTime(startTime);
		}
	}
}

