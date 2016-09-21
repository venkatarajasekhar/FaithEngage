using System;

namespace FaithEngage.Core.DisplayUnits
{
    /// <summary>
    /// This is a grouping struct used to associate a given Display Unit with its grouping.
	/// A DisplayUnitGrouping has an id to make it unique and a position in grouping. This
	/// allows all DisplayUnits grouped together to be associated with a single group and ordered
	/// within that group.
    /// </summary>
	public struct DisplayUnitGrouping
    {
        public DisplayUnitGrouping (int position, Guid id)
        {
            Position = position;
            Id = id;
        }
		/// <summary>
		/// The position in the grouping. Used to order DisplayUnits by group.
		/// </summary>
        public int Position;
		/// <summary>
		/// The unique grouping identifier. 
		/// </summary>
        public Guid Id;
    }
}

