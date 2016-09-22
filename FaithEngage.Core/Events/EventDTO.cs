using System;
namespace FaithEngage.Core.Events
{
	/// <summary>
	/// A data transfer object for Events.
	/// </summary>
	public class EventDTO
	{
        public Guid EventId { get; set;}
        public Guid AssociatedOrg { get; set;}
        public Guid EventScheduleId { get; set;}
        public DateTime? UtcEventDate { get; set;}
	}
}

