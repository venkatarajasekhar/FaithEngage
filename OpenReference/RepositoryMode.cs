using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Scripture
{
    /// <summary>
    /// Represents the method by which the repository handles improperly formatted verse s.
    /// </summary>
    public enum RepositoryMode
    {
        /// <summary>
        /// Returns any fully qualified verse data based on exact s provided.
        /// Excludes results for any improperly formatted s instead of throwing an exception.
        /// </summary>
        Safe,
        /// <summary>
        /// Returns all fully qualified verse data based on exact s provided.
        /// Throws an exception on any improperly formatted s.
        /// </summary>
        Strict,
        /// <summary>
        /// Returns fully qualified verse data based on information provided.
        /// Includes best guess results for any improperly formatted s.
        /// </summary>
        Optimistic
    }

    public enum VersePosition
    {
        First,
        Last
    }
}
