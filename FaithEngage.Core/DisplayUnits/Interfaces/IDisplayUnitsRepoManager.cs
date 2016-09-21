using System;
using System.Collections.Generic;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    /// <summary>
    /// The repository for display unit access
    /// </summary>
	public interface IDisplayUnitsRepoManager
    {
        /// <summary>
        /// Obtains a DisplayUnit by its id.
        /// </summary>
		/// <returns>The Display unit or null (if inaccessible)</returns>
        /// <param name="unitId">The DisplayUnit's ID</param>
		DisplayUnit GetById (Guid unitId);
		/// <summary>
		/// Obtains a Dictionary of DisplayUnits for the specified Event. If onlyPushed
		/// is true, will only return a dictionary of DisplayUnits that have been pushed live.
		/// 
		/// The key in the dictionary corresponds to the position the Display unit has in the event.
		/// </summary>
		/// <returns>A dictionary&lt;position,DisplayUnit></returns>
		/// <param name="eventId">The event identifier.</param>
		/// <param name="onlyPushed">If set to <c>true</c>, will only return the events that are currently live.</param>
        Dictionary<int,DisplayUnit> GetByEvent (Guid eventId, bool onlyPushed = false);
        /// <summary>
        /// This will save a series of DisplayUnits to an event, each in the position specified by the int
		/// key.
        /// </summary>
        /// <param name="unitsAtPositions">Units at positions.</param>
        /// <param name="eventId">Service identifier.</param>
		void SaveManyToEvent (Dictionary<int,DisplayUnit> unitsAtPositions, Guid eventId);
        /// <summary>
        /// Saves a DisplayUnit to its associated event.
        /// </summary>
        /// <param name="unit">The DisplayUnit</param>
		void SaveOneToEvent (DisplayUnit unit);
        /// <summary>
        /// Saves the DisplayUnitDto to its associated event. 
        /// </summary>
        /// <param name="dto">Dto.</param>
		void SaveDtoToEvent(DisplayUnitDTO dto);
        
		//TODO: Figure out if this is necessary and explore better use/implementation
		void DuplicateToEvent(DisplayUnit unit);
        /// <summary>
        /// Deletes a specified display unit
        /// </summary>
        /// <param name="unitId">Unit identifier.</param>
		void Delete (Guid unitId);
        /// <summary>
        /// Pushes a DisplayUnit live, specified by its id.
        /// </summary>
        /// <returns>The DisplayUnit being pushed</returns>
        /// <param name="id">Identifier.</param>
		DisplayUnit PushDU (Guid id);
        /// <summary>
        /// Pulls a DisplayUnit that is currently live.
        /// </summary>
        /// <returns>The DisplayUnit that is being pulled</returns>
        /// <param name="id">Identifier.</param>
		DisplayUnit PullDu (Guid id);

		//TODO: Evaluate this method. Is it necessary? What is the purpose?
		/// <summary>
		/// Obtains a Display Unit Group specified by the group id and the event to which they belong.
		/// </summary>
		/// <returns>The group.</returns>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="groupId">Group identifier.</param>
        Dictionary<int,DisplayUnit> GetGroup (Guid eventId, Guid groupId);
    }
}

