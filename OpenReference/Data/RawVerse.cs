using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Konves.Scripture.Data
{
    /// <summary>
    /// Represents the raw combination of an optional book and up to two optional numbers found in a single verse range.
    /// </summary>
    internal class RawVerse
    {
        #region Parse
        /// <summary>
        /// Converts the string representation of a verse to its RawVerse equivalent.
        /// </summary>
        /// <param name="s">A string containing a verse to convert.</param>
        /// <returns>
        /// A RawVerse equivalent to the verse contained in <paramref name="s"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s"/> is null.</exception>
        internal static RawVerse Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException("s is null");

            RawVerse result = new RawVerse();
            Regex regex = new Regex(
                @"(?<book>(?:[1-3]\s?)?[a-z][a-z\s]+)?(?<n1>\s*[0-9]+[a-c]?\s*)?(?::(?<n2>\s*[0-9]+[a-c]?\s*))?",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase
            );

            Match match = regex.Match(input.Trim());

            if (match == null || string.IsNullOrWhiteSpace(match.Value))
            {
                result.Book = null;
                result.N1 = null;
                result.N2 = null;

                result.HasValue = false;
                result.NumberCount = 0;
            }
            else
            {
                result.Book = match.Groups["book"] == null || string.IsNullOrWhiteSpace(match.Groups["book"].Value) ? null : match.Groups["book"].Value.Trim();
                result.N1 = match.Groups["n1"] == null || string.IsNullOrWhiteSpace(match.Groups["n1"].Value) ? null : match.Groups["n1"].Value.Trim();
                result.N2 = match.Groups["n2"] == null || string.IsNullOrWhiteSpace(match.Groups["n2"].Value) ? null : match.Groups["n2"].Value.Trim();

                result.NumberCount = (result.N1 == null ? 0 : 1) + (result.N2 == null ? 0 : 1);

                result.HasValue = true;
            }

            return result;
        } 
        #endregion

        #region internal Properties
        /// <summary>
        /// Gets or sets a value indicating whether a book or any number values were found in this verse.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instances has value; otherwise, <c>false</c>.
        /// </value>
        internal bool HasValue { get; set; }
        /// <summary>
        /// Gets or sets the value of the book value of this verse or <c>null</c> if no book was found.
        /// </summary>
        /// <value>
        /// The book.
        /// </value>
        internal string Book { get; set; }
        /// <summary>
        /// Gets or sets the first number value found in this verse or <c>null</c> if no numbers were found.
        /// </summary>
        /// <value>
        /// The first number.
        /// </value>
        internal string N1 { get; set; }
        /// <summary>
        /// Gets or sets the second number value found in this verse or <c>null</c> if fewer than two numbers were found.
        /// </summary>
        /// <value>
        /// The second number.
        /// </value>
        internal string N2 { get; set; }
        /// <summary>
        /// Gets or sets the count of number values found in this verse.
        /// </summary>
        /// <value>
        /// The numbers.
        /// </value>
        internal int NumberCount { get; set; } 
        #endregion

        #region Equality Overrides
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instances.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instances.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instances; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instances.
        /// </summary>
        /// <returns>
        /// A hash code for this instances, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return
                this.HasValue.GetHashCode()
                ^ (Book == null ? 0 : Book.ToLower().GetHashCode())
                ^ (N1 == null ? 0 : N1.ToLower().GetHashCode() >> 1)
                ^ (N2 == null ? 0 : N2.ToLower().GetHashCode() << 1)
                ^ (NumberCount);
        } 
        #endregion
    }
}
