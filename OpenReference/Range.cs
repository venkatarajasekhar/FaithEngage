using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konves.Scripture.Version;

namespace Konves.Scripture
{
    /// <summary>
    /// Represents a range of scripture verses.
    /// </summary>
    internal class Range : IFormattable, IComparable , IComparable<Range>
    {
        private Version.ScriptureInfo _scriptureInfo;
        private IEnumerable<OffsetChapterLimits> _offsetChapterLimits;
        
        internal static Range Create(ScriptureInfo scriptureInfo, Data.RawRange range, int offset, RepositoryMode mode)
        {
            Range result = new Range();

            result._scriptureInfo = scriptureInfo;
            result.Offset = offset;

            result.Start = Verse.Create(scriptureInfo, range.FirstBook, range.FirstChapter, range.FirstVerse, range.FirstVerseSuffix, VersePosition.First, mode);
            result.End = Verse.Create(scriptureInfo, range.SecondBook, range.SecondChapter, range.SecondVerse, range.SecondVerseSuffix, VersePosition.Last, mode);

            int firstChapterLimitsOffset = 1 - result.Start.VerseNumber;

            result._offsetChapterLimits =
                OffsetChapterLimits.Create(
                    firstChapterLimitsOffset,
                    scriptureInfo.ChapterLimits.Where(c => result.Start <= c && c <= result.End)
                    );

            result.Length =
                result._offsetChapterLimits.Select(c => c.ChapterLimits).Sum(c => c.EndVerseNumber)
                + firstChapterLimitsOffset
                - (result._offsetChapterLimits.Last().ChapterLimits.EndVerseNumber - result.End.VerseNumber);

            if (mode != RepositoryMode.Optimistic && (result.Start == null || result.End == null))
                result = null;

            return result;
        }

        public Verse this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Length)
                    throw new IndexOutOfRangeException("index is greater than or equal to the length of this object or less than zero.");

                // TODO: perfomance tune this line for large ranges
                var c = this._offsetChapterLimits.Last(x => x.Offset <= index);

                var bi = _scriptureInfo.GetBookInfo(c.ChapterLimits.BookNumber);

                Verse verse = Verse.Create(_scriptureInfo, bi, c.ChapterLimits.ChapterNumber, index - c.Offset + 1, null, VersePosition.First, RepositoryMode.Safe);
                
                return verse;
            }
        }

        /// <summary>
        /// Gets a the verse at the start of this range.
        /// </summary>
        /// <value>
        /// The starting verse.
        /// </value>
        public Verse Start { get; internal set; }

        /// <summary>
        /// Gets the verse at the end of this range.
        /// </summary>
        /// <value>
        /// The ending verse.
        /// </value>
        public Verse End { get; internal set; }

        public int Offset { get; internal set; }

        /// <summary>
        /// Gets or sets the number of verses in this range.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Length { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instances.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instances.
        /// </returns>
        public override string ToString()
        {
            return this.ToString(null, null);

            //// TODO: add funtionality to create short strings: "Genesis 1-6" or "Matthew-John"

            //if (Start != null && End != null)
            //{
            //    if (Start.BookNumber == End.BookNumber && Start.ChapterNumber == End.ChapterNumber && Start.VerseNumber == End.VerseNumber)
            //    {
            //        return Start.ToString();
            //    }
            //    else if (Start.BookNumber == End.BookNumber && Start.ChapterNumber == End.ChapterNumber)
            //    {
            //        return Start.BookName + " " + Start.ChapterNumber + ":" + Start.VerseNumber + Start.Suffix + "-" + End.VerseNumber + End.Suffix;
            //    }
            //    else if (Start.BookNumber == End.BookNumber )
            //    {
            //        return Start.BookName + " " + Start.ChapterNumber + ":" + Start.VerseNumber + Start.Suffix + "-" + End.ChapterNumber + ":" + End.VerseNumber + End.Suffix;
            //    }
            //    else
            //        return Start.ToString() + " - " + End.ToString();
            //}
            //else
            //    return base.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            IFormatProvider provider = formatProvider ?? DefaultFormatProvider.Instance;

            object obj = provider.GetFormat(this.GetType());

            if (obj is ICustomFormatter)
            {
                return (obj as ICustomFormatter).Format(format, this, provider);
            }
            else
                return null;
        }

        /// <summary>
        /// Represent a <see cref="ChapterLimits"/> object at an offest to the first <see cref="Verse"/> in a <see cref="Range"/>.
        /// </summary>
        private class OffsetChapterLimits
        {
            /// <summary>
            /// Prevents a default instance of the <see cref="OffsetChapterLimits"/> class from being created.
            /// </summary>
            private OffsetChapterLimits() { }

            /// <summary>
            /// Gets or sets the offset from the first verse in a <see cref="Range"/> to the first verse in this instance.
            /// A negative value indicates that the first verse of the underlying <see cref="ChapterLimits"/> object
            /// falls before the first verse of the <see cref="Range"/> while a positive value indicates that the
            /// first verse of the underlying <see cref="ChapterLimits"/> object falls after the first verse
            /// of the <see cref="Range"/>.
            /// </summary>
            /// <value>
            /// The offset value.
            /// </value>
            public int Offset { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ChapterLimits"/> object.
            /// </summary>
            /// <value>
            /// The chapter limits.
            /// </value>
            public ChapterLimits ChapterLimits { get; set; }

            public static IEnumerable<OffsetChapterLimits> Create(int firstOffset, IEnumerable<ChapterLimits> chapterLimits)
            {
                int offset = firstOffset;

                foreach (ChapterLimits c in chapterLimits)
                {
                    yield return new OffsetChapterLimits
                    {
                        ChapterLimits = c,
                        Offset = offset
                    };
                    // TODO: account for omitted verses
                    offset += c.EndVerseNumber;
                }
            }
        }

        ///// <summary>
        ///// Determines whether this <see cref="Range"/> contains the specified <see cref="Verse"/>.
        ///// </summary>
        ///// <param name="verse">The <see cref="Verse"/>.</param>
        ///// <returns>
        /////   <c>true</c> if this <see cref="Range"/> contains the specified <see cref="Verse"/>; otherwise, <c>false</c>.
        ///// </returns>
        ///// <exception cref="ArgumentNullException">
        /////     <paramref name="verse"/> is null.</exception>
        //public bool Contains(Verse verse)
        //{
        //    if (verse == null)
        //        throw new ArgumentNullException("verse", "verse is null");

        //    return this.Start.Index <= verse.Index && this.End.BookNumber >= verse.Index;
        //}

        ///// <summary>
        ///// Determines whether this <see cref="Range"/> contains all of the <see cref="Verse">Verses</see>
        ///// in the specified <see cref="Range"/>.
        ///// </summary>
        ///// <param name="range">The <see cref="Range"/>.</param>
        ///// <returns>
        /////   <c>true</c> if this <see cref="Range"/> contains all of the <see cref="Verse">Verses</see>
        ///// in the specified <see cref="Range"/>; otherwise, <c>false</c>.
        ///// </returns>
        ///// <exception cref="ArgumentNullException">
        /////     <paramref name="range"/> is null.</exception>
        //public bool Contains(Range range)
        //{
        //    if (range == null)
        //        throw new ArgumentNullException("range", "range is null");

        //    return this.Start.Index <= range.Start.Index && this.End.BookNumber >= range.End.Index;
        //}

        /////// <summary>
        /////// Determines whether this <see cref="Range"/> contains all of the <see cref="Verse">Verses</see>
        /////// in the specified <see cref="RangeCollection"/>.
        /////// </summary>
        /////// <param name="range">The <see cref="RangeCollection"/>.</param>
        /////// <returns>
        ///////   <c>true</c> if this <see cref="Range"/> contains all of the <see cref="Verse">Verses</see>
        /////// in the specified <see cref="RangeCollection"/>; otherwise, <c>false</c>.
        /////// </returns>
        /////// <exception cref="ArgumentNullException">
        ///////     <paramref name="range"/> is null.</exception>
        ////public bool Contains(RangeCollection reference)
        ////{
        ////    if (reference == null)
        ////        throw new ArgumentNullException("range", "range is null");

        ////    return reference.All(range => this.Contains(range));
        ////}

        ///// <summary>
        ///// Determines whether this <see cref="Range"/> contains any of the <see cref="Verse">Verses</see>
        ///// in the specified <see cref="Range"/>.
        ///// </summary>
        ///// <param name="range">The <see cref="Range"/>.</param>
        ///// <returns>
        /////   <c>true</c> if this <see cref="Range"/> contains any of the <see cref="Verse">Verses</see>
        ///// in the specified <see cref="Range"/>; otherwise, <c>false</c>.
        ///// </returns>
        ///// <exception cref="ArgumentNullException">
        /////     <paramref name="range"/> is null.</exception>
        //public bool Intersects(Range range)
        //{
        //    if (range == null)
        //        throw new ArgumentNullException("range", "range is null");

        //    return this.Contains(range.Start) || this.Contains(range.End);
        //}

        ///// <summary>
        ///// Determines whether this <see cref="Range"/> contains any of the <see cref="Verse">Verses</see>
        ///// in the specified <see cref="RangeCollection"/>.
        ///// </summary>
        ///// <param name="range">The <see cref="RangeCollection"/>.</param>
        ///// <returns>
        /////   <c>true</c> if this <see cref="Range"/> contains any of the <see cref="Verse">Verses</see>
        ///// in the specified <see cref="RangeCollection"/>; otherwise, <c>false</c>.
        ///// </returns>
        ///// <exception cref="ArgumentNullException">
        /////     <paramref name="range"/> is null.</exception>
        //public bool Intersects(RangeCollection reference)
        //{
        //    if (reference == null)
        //        throw new ArgumentNullException("range", "range is null");

        //    return reference.Any(range => this.Intersects(range));
        //}
        
        ///// <summary>
        ///// Determines whether the specified <see cref="System.Object"/> is equal to this instances.
        ///// </summary>
        ///// <param name="obj">The <see cref="System.Object"/> to compare with this instances.</param>
        ///// <returns>
        /////   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instances; otherwise, <c>false</c>.
        ///// </returns>
        //public override bool Equals(object obj)
        //{
        //    // TODO: move to an IEqualityComparer
        //    return this.GetHashCode() == obj.GetHashCode();
        //}

        ///// <summary>
        ///// Returns a hash code for this instances.
        ///// </summary>
        ///// <returns>
        ///// A hash code for this instances, suitable for use in hashing algorithms and data structures like a hash table. 
        ///// </returns>
        //public override int GetHashCode()
        //{
        //    // TODO: move to an IEqualityComparer
        //    return
        //        Start.GetHashCode()
        //        ^ (End.GetHashCode() >> 1);
        //}

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Range)
                return this.CompareTo(obj as Range);
            else
                throw new ArgumentException("obj is not a Range");
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(Range other)
        {
            if (other == null)
                return 1;

            int i = this.Start.CompareTo(other.Start);

            if (i == 0)
                return this.End.CompareTo(other.End);
            else
                return i;

        }
    }
}
