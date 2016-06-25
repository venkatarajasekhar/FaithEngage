using System;
namespace FaithEngage.Core.Events
{
	public class EventDTO
	{
        public Guid EventId { get; set;}
        public Guid AssociatedOrg { get; set;}
        public Guid EventScheduleId { get; set;}
        public DateTime EventDate { get; set;}
	}
}

