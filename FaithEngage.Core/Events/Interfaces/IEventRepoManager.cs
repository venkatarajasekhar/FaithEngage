using System;
using System.Collections.Generic;

namespace FaithEngage.Core.Events.Interfaces
{
	/// <summary>
	/// The manager for the Event repository, providing internal access to the data, regardless
	/// of the repository.
	/// </summary>
	public interface IEventRepoManager
	{
		/// <summary>
		/// Gets an event by its id.
		/// </summary>
		/// <returns>An Event, if found. Null, if not.</returns>
		/// <param name="id">Identifier.</param>
		Event GetById(Guid id);
		/// <summary>
		/// Obtains a list of events for a given date and organization id.
		/// If the organization id is not found, will throw an exception.
		/// </summary>
		/// <returns>A list of Events.</returns>
		/// <param name="date">The date to search.</param>
		/// <param name="orgId">Organization identifier.</param>
		IList<Event> GetByDate (DateTimeOffset date, Guid orgId);
		/// <summary>
		/// Gets a list of events associated with a given organization id.
		/// If the organization id is not found, will throw an exception.
		/// </summary>
		/// <returns>A list of Events</returns>
		/// <param name="orgId">The organization's id</param>
		IList<Event> GetByOrgId (Guid orgId);
		/// <summary>
		/// Saves a new event to the repository.
		/// </summary>
		/// <returns>The new event's id</returns>
		/// <param name="eventToSave">Event to save.</param>
		Guid SaveEvent(Event eventToSave);
		/// <summary>
		/// Updates a pre-existing event in the repository
		/// </summary>
		/// <param name="eventToUpdate">Event to update.</param>
		void UpdateEvent(Event eventToUpdate);
		/// <summary>
		/// Deletes a pre-existing event in the repository
		/// </summary>
		/// <param name="id">Identifier.</param>
		void DeleteEvent(Guid id);
	}
}

