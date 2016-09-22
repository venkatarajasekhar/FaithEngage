using System;
using NUnit.Framework;
namespace FaithEngage.Core.Events.EventSchedules
{
	[TestFixture]
	public class EventScheduleTests
	{
		private DateTimeOffset startTime = new DateTimeOffset(DateTime.Parse("10:00am"), TimeZoneInfo.Local.BaseUtcOffset);
		private DateTimeOffset endTime = new DateTimeOffset(DateTime.Parse("12:00pm"), TimeZoneInfo.Local.BaseUtcOffset);

		[Test]
		public void SetUTCStartTime_ValidStartTime_GivesUTCStartTime()
		{
			var sched = new EventSchedule();
			sched.SetStartTime(startTime);

			var time = sched.UTCStartTime;

			Assert.That(time, Is.EqualTo(startTime.UtcDateTime.TimeOfDay));
		}

		[Test]
		public void SetUTCStartTime_EmptyStartTime_GivesUTCStartTimeOfMidNight()
		{
			var sched = new EventSchedule();
			sched.SetStartTime(new DateTimeOffset());

			var time = sched.UTCStartTime;

			Assert.That(time, Is.EqualTo(new TimeSpan()));
		}

		[Test]
		public void SetUTCEndTime_ValidStartTime_GivesUTCStartTime()
		{
			var sched = new EventSchedule();
			sched.SetEndTime(endTime);

			var time = sched.UTCEndTime;

			Assert.That(time, Is.EqualTo(endTime.UtcDateTime.TimeOfDay));
		}

		[Test]
		public void SetUTCEndTime_EmptyStartTime_GivesUTCStartTimeOfMidNight()
		{
			var sched = new EventSchedule();
			sched.SetEndTime(new DateTimeOffset());

			var time = sched.UTCEndTime;

			Assert.That(time, Is.EqualTo(new TimeSpan()));
		}


	}
}

