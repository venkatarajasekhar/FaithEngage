using System;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Events.EventSchedules.Factories
{
    /// <summary>
    /// Converts EventSchedules to EventScheduleDTOs.
    /// </summary>
	public class EventScheduleDTOFactory : IConverterFactory<EventSchedule,EventScheduleDTO>
	{
		/// <summary>
		/// Converts an EventSchedule to an EventScheduleDTO. All DateTimes are converted to universal time.
		/// </summary>
		/// <param name="evnt">The event to be converted.</param>
		public EventScheduleDTO Convert (EventSchedule evnt)
        {
            var dto = new EventScheduleDTO (); 
            dto.Day = evnt.Day;
            dto.EventDescription = evnt.EventDescription;
            dto.EventName = evnt.EventName;
            dto.Id = evnt.Id;
            dto.OrgId = evnt.Id;
            dto.Recurrance = evnt.Recurrance;
			dto.UTCRecurringEnd = evnt.RecurringEnd.ToUniversalTime().DateTime;
            dto.UTCRecurringStart = evnt.RecurringStart.ToUniversalTime().DateTime;
            dto.UTCEndTime = evnt.UTCEndTime;
            dto.UTCStartTime = evnt.UTCStartTime;
            return dto;
        }
    }
}

