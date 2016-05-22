using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konves.Scripture.Data;

namespace Konves.Scripture
{
    /// <summary>
    /// Represents a single scripture verse.
    /// </summary>
    public class Verse : IFormattable, IComparable, IComparable<Verse>
    {
        private Scripture.Version.ScriptureInfo _scriptureInfo;
        private BookInfo _bookInfo;

        internal static Verse Create(Scripture.Version.ScriptureInfo scriptureInfo, string book, int chapter, int verse, string suffix, VersePosition position, RepositoryMode mode)
        {
            BookInfo bookInfo = scriptureInfo.GetBookInfo(book);

            return Create(scriptureInfo, bookInfo, chapter, verse, suffix, position, mode);
        }

        internal static Verse Create(Scripture.Version.ScriptureInfo scriptureInfo, BookInfo bookInfo, int chapter, int verse, string suffix, VersePosition position, RepositoryMode mode)
        {
            Verse result = new Verse();

            result._scriptureInfo = scriptureInfo;

            result._bookInfo = bookInfo; // scriptureInfo.GetBookInfo(bookNumber);

            if (result._bookInfo == null)
            {
                result = null;
            }
            else
            {
                switch (position)
                {
                    case VersePosition.First:
                        if (chapter < 1)
                        {
                            if (chapter == -1 || mode == RepositoryMode.Optimistic)
                            {
                                chapter = 1;
                                verse = -1;
                            }
                            else
                                result = null;
                        }

                        if (result != null && verse < 1)
                        {
                            if (verse == -1 || mode == RepositoryMode.Optimistic)
                                verse = 1;
                            else
                                result = null;
                        }
                        break;
                    case VersePosition.Last:
                        if (chapter < 1)
                        {
                            if (chapter == -1 || mode == RepositoryMode.Optimistic)
                            {
                                chapter = scriptureInfo.GetLastChapter(result._bookInfo.Number);
                                verse = -1;
                            }
                            else
                                result = null;
                        }

                        if (result != null && verse < 0)
                        {
                            if (verse == -1 || mode == RepositoryMode.Optimistic)
                            {
                                try
                                {
                                    verse = scriptureInfo.GetLastVerse(result._bookInfo.Number, chapter);
                                }
                                catch (Exception)
                                {
                                    if (mode == RepositoryMode.Optimistic)
                                    {
                                        chapter = scriptureInfo.GetLastChapter(result._bookInfo.Number);
                                        verse = scriptureInfo.GetLastVerse(result._bookInfo.Number, chapter);
                                    }
                                    else
                                        result = null;

                                }
                            }
                            else
                                result = null;
                        }

                        break;
                }

                int maxChapter = scriptureInfo.GetLastChapter(result._bookInfo.Number);
                if (chapter > maxChapter)
                {
                    if (mode == RepositoryMode.Optimistic)
                    {
                        chapter = maxChapter;
                        verse = int.MaxValue;
                    }
                    else
                        result = null;
                }
                if (result != null)
                {
                    int maxVerse = scriptureInfo.GetLastVerse(result._bookInfo.Number, chapter);
                    if (verse > maxVerse)
                    {
                        if (mode == RepositoryMode.Optimistic)
                            verse = maxVerse;
                        else
                            result = null;
                    }
                }

                if (result != null)
                {

                    int index = -1;
                    int chapterResult = -1;
                    int verseResult = -1;

                    scriptureInfo.GetVerseData(result._bookInfo.Number, chapter, verse, out index, out chapterResult, out verseResult);
                    
                    result.Index = index; 
                    result.ChapterNumber = chapterResult; 
                    result.VerseNumber = verseResult; 
                    result.Suffix = string.IsNullOrWhiteSpace(suffix) ? null : suffix.Trim().ToLower();
                }

            }

            return result;
        }
        
        /// <summary>
        /// Gets the verse index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the name of the book.
        /// </summary>
        /// <value>
        /// The book.
        /// </value>
        public string BookName
        {
            get
            {
                return _bookInfo == null ? null : _bookInfo.Name;
            }
            internal set
            {
                if (_bookInfo == null)
                    _bookInfo = new BookInfo();

                _bookInfo.Name = value;
            }
        }

        /// <summary>
        /// Gets the one-based book number.
        /// </summary>
        /// <value>
        /// The book number.
        /// </value>
        public int BookNumber
        {
            get
            {
                return _bookInfo == null ? 0 : _bookInfo.Number;
            }
            internal set
            {
                if (_bookInfo == null)
                    _bookInfo = new BookInfo();

                _bookInfo.Number = value;
            }
        }

        /// <summary>
        /// Gets the one-based chapter number.
        /// </summary>
        /// <value>
        /// The chapter number.
        /// </value>
        public int ChapterNumber { get; internal set; }

        /// <summary>
        /// Gets the one-based verse number.
        /// </summary>
        /// <value>
        /// The verse number.
        /// </value>
        public int VerseNumber { get; internal set; }

        /// <summary>
        /// Gets an optional verse suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix { get; internal set; }

        public string TranslationName
        {
            get
            {
                return _scriptureInfo.TranslationName;
            }
        }

        public string TranslationAbbreviation
        {
            get
            {
                return _scriptureInfo.TranslationAbbreviation;
            }
        }

        public string ScriptureName
        {
            get
            {
                return _scriptureInfo.Name;
            }
        }

        public string ShortBookAbbreviation
        {
            get
            {
                return _bookInfo == null ? null : _bookInfo.ShortAbbr;
            }
        }

        public string LongBookAbbreviation
        {
            get
            {
                return _bookInfo == null ? null : _bookInfo.LongAbbr;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instances.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instances.
        /// </returns>
        public override string ToString()
        {
            //return this.BookName + " " + this.ChapterNumber + ":" + this.VerseNumber + this.Suffix;

            return this.ToString(VerseFormatter.DefaultFormat, null);
        }

        ///// <summary>
        ///// Implements the operator ==.
        ///// </summary>
        ///// <param name="v1">The v1.</param>
        ///// <param name="v2">The v2.</param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static bool operator ==(Verse v1, Verse v2)
        //{
        //    return v1.Equals(v2);
        //}

        ///// <summary>
        ///// Implements the operator !=.
        ///// </summary>
        ///// <param name="v1">The v1.</param>
        ///// <param name="v2">The v2.</param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static bool operator !=(Verse v1, Verse v2)
        //{
        //    return !v1.Equals(v2);
        //}

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Verse v1, Verse v2)
        {
            return v1.Index < v2.Index;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Verse v1, Verse v2)
        {
            return v1.Index > v2.Index;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instances.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instances.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instances; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            // TODO: implement in an IEqualityComparer
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
            // TODO: implement in an IEqualityComparer
            return
                ((this.Index * 100) >> 4)
                ^ ((this.BookNumber * 50) << 4)
                ^ (this.BookName == null ? 0 : this.BookName.GetHashCode())
                ^ (this.ChapterNumber >> 3)
                ^ (this.VerseNumber << 3)
                ^ (this.Suffix == null ? 0 : this.Suffix.GetHashCode());
        }

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


        #region Verse/ChapterLimits operators

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Verse"/> falls before and is not included in the specified <see cref="ChapterLimits"/>.
        /// </summary>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Verse v, Version.ChapterLimits c)
        {
            return
                v.BookNumber < c.BookNumber
                ||
                (v.BookNumber == c.BookNumber && v.ChapterNumber < c.ChapterNumber);
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="ChapterLimits"/> falls after and does not include the specified <see cref="Verse"/>.
        /// </summary>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Version.ChapterLimits c, Verse v)
        {
            return v < c;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Verse"/> falls after and is not included in the specified <see cref="ChapterLimits"/>.
        /// </summary>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Verse v, Version.ChapterLimits c)
        {
            return
                v.BookNumber > c.BookNumber
                ||
                (v.BookNumber == c.BookNumber && v.ChapterNumber > c.ChapterNumber);
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="ChapterLimits"/> falls before and does not include the specified <see cref="Verse"/>.
        /// </summary>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Version.ChapterLimits c, Verse v)
        {
            return v > c;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Verse"/> is included in the specified <see cref="ChapterLimits"/>.
        /// </summary>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Verse v, Version.ChapterLimits c)
        {
            if (object.ReferenceEquals(null, c))
                return object.ReferenceEquals(v, null);
            return
                v.BookNumber == c.BookNumber
                && v.ChapterNumber == c.ChapterNumber;
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="ChapterLimits"/> includes the specified <see cref="Verse"/>.
        /// </summary>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Version.ChapterLimits c, Verse v)
        {
            if (object.ReferenceEquals(null, c))
                return object.ReferenceEquals(v, null);
            else
                return v == c;
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Verse"/> is not included in the specified <see cref="ChapterLimits"/>.
        /// </summary>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Verse v, Version.ChapterLimits c)
        {
            if (object.ReferenceEquals(null, c))
                return !object.ReferenceEquals(v, null);
            else
                return
                    v.BookNumber != c.BookNumber
                    || v.ChapterNumber != c.ChapterNumber;
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="ChapterLimits"/> does not include the specified <see cref="Verse"/>.
        /// </summary>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Version.ChapterLimits c, Verse v)
        {
            if (object.ReferenceEquals(null, c))
                return !object.ReferenceEquals(v, null);
            return v != c;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Verse"/> falls before or is included in the specified <see cref="ChapterLimits"/>.
        /// </summary>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(Verse v, Version.ChapterLimits c)
        {
            return v < c || v == c;
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="ChapterLimits"/> falls after or includes the specified <see cref="Verse"/>.
        /// </summary>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(Version.ChapterLimits c, Verse v)
        {
            return v <= c;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Verse"/> falls after or is included in the specified <see cref="ChapterLimits"/>.
        /// </summary>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(Verse v, Version.ChapterLimits c)
        {
            return v > c || v == c;
        }
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="ChapterLimits"/> falls before or includes the specified <see cref="Verse"/>.
        /// </summary>
        /// <param name="c">The <see cref="ChapterLimits"/>.</param>
        /// <param name="v">The <see cref="Verse"/>.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(Version.ChapterLimits c, Verse v)
        {
            return v >= c;
        }
        #endregion


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

            if (obj is Verse)
                return this.CompareTo(obj as Verse);
            else
                throw new ArgumentException("obj is not a Verse");
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
        public int CompareTo(Verse other)
        {
            if (other == null)
                return 1;

            else if (this.BookNumber > other.BookNumber)
                return 1;
            else if (this.BookNumber < other.BookNumber)
                return -1;
            else if (this.ChapterNumber > other.ChapterNumber)
                return 1;
            else if (this.ChapterNumber < other.ChapterNumber)
                return -1;
            else if (this.VerseNumber > other.VerseNumber)
                return 1;
            else if (this.VerseNumber < other.VerseNumber)
                return -1;
            else
                // TODO: compare suffix
                return 0;

        }
    }
}
