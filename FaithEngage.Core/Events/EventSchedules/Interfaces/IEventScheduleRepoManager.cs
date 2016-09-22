using System;
using System.Collections.Generic;

namespace FaithEngage.Core.Events.EventSchedules.Interfaces
{
	/// <summary>
	/// The Manager for the EventSchedule Repository. Provides internal access to the
	/// repository.
	/// </summary>
	public interface IEventScheduleRepoManager
	{
		/// <summary>
		/// Gets an EventSchedule by the identifier.
		/// </summary>
		/// <returns>An event schedule (if found) or null (if not)</returns>
		/// <param name="id">Identifier.</param>
		EventSchedule GetById (Guid id);
		/// <summary>
		/// Gets a list of EventSchedules by the organization ID
		/// </summary>
		/// <returns>A list of EventSchedules.</returns>
		/// <param name="id">Identifier.</param>
		IList<EventSchedule> GetByOrgId (Guid id);
		/// <summary>
		/// Saves a new EventSchedule to the repository.
		/// </summary>
		/// <returns>The id of the event schedule</returns>
		/// <param name="schedule">Schedule.</param>
		Guid SaveSchedule (EventSchedule schedule);
		/// <summary>
		/// Saves an updated event schedule (of  a preexisting schedule)
		/// to the repository.
		/// </summary>
		/// <param name="schedule">Schedule.</param>
		void UpdateSchedule (EventSchedule schedule);
		/// <summary>
		/// Deletes the identified schedule from the repository.
		/// </summary>
		/// <param name="id">Identifier.</param>
		void DeleteSchedule (Guid id);
	}
}

