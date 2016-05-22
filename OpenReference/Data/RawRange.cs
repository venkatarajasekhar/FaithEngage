using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Konves.Scripture.Data
{
    /// <summary>
    /// Represents a range of verses.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FirstBook} {FirstChapter}:{FirstVerseString} - {SecondBook} {SecondChapter}:{SecondVerseString}")]
    internal class RawRange
    {
        #region Parse
        /// <summary>
        /// Converts the string representation of a verse range to its RawRange equivalent.
        /// </summary>
        /// <param name="s">A string containing a verse range to convert.</param>
        /// <param name="currentBook">The current book.</param>
        /// <param name="currentChapter">The current chapter.</param>
        /// <param name="currentVerse">The current verse.</param>
        /// <returns>
        /// A RawRange equivalent to the verse range contained in <paramref name="s"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="s"/> is null.</exception>
        internal static RawRange Parse(string input, string currentBook, string currentChapter, string currentVerse)
        {
            if (input == null)
                throw new ArgumentNullException("s is null.");

            if (string.IsNullOrWhiteSpace(currentBook))
                currentBook = null;

            if (string.IsNullOrWhiteSpace(currentChapter))
                currentChapter = null;

            if (string.IsNullOrWhiteSpace(currentVerse))
                currentVerse = null;

            RawRange result = new RawRange();

            string[] segs = input.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            string seg0 = segs[0];
            string seg1 = segs.Length == 2 ? segs[1] : string.Empty;

            RawVerse first = RawVerse.Parse(seg0);
            RawVerse second = RawVerse.Parse(seg1);

            if (!first.HasValue)
                throw new FormatException("s is not in the correct format.");
            else
            {
                // Set FirstBook
                if (first.Book == null && currentBook == null)
                    throw new ArgumentNullException("currentBook", "currentBook cannot be null when a book is not provided by s");
                else if (first.Book != null)
                {
                    result.FirstBook = first.Book;
                    result.IsFirstBookExplicit = true;
                    currentChapter = null;
                    currentVerse = null;
                }
                else
                {
                    result.FirstBook = currentBook;
                    result.IsFirstBookExplicit = false;
                }
                
                // Set ChapterNumber and VerseNumber
                if (first.NumberCount == 1)
                {
                    //if (string.IsNullOrEmpty(currentChapter))
                    if (string.IsNullOrEmpty(currentVerse))
                    {
                        result.FirstChapterString = first.N1;
                        result.SecondChapterString = null;
                    }
                    else
                    {
                        result.FirstChapterString = currentChapter;
                        result.FirstVerseString = first.N1;
                    }
                }
                else
                {
                    result.FirstChapterString = first.N1;
                    result.FirstVerseString = first.N2;
                }
            }
            
            // Parse second segment
            if (!second.HasValue)
            {
                result.SecondBook = result.FirstBook;
                result.SecondChapterString = result.FirstChapterString;
                result.SecondVerseString = result.FirstVerseString;
            }
            else
            {
                result.SecondBook = second.Book == null ? result.FirstBook : second.Book;

                if (second.NumberCount == 0)
                {
                    result.SecondChapterString = result.FirstChapterString;
                    result.SecondVerseString = result.FirstVerseString;
                }
                else if (second.NumberCount == 1)
                {
                    if (string.IsNullOrWhiteSpace(result.FirstVerseString))
                    {
                        result.SecondChapterString = second.N1;
                        result.SecondVerseString = null;
                    }
                    else
                    {
                        result.SecondChapterString = result.FirstChapterString;
                        result.SecondVerseString = second.N1;
                    }
                }
                else
                {
                    result.SecondChapterString = second.N1;
                    result.SecondVerseString = second.N2;
                }
            }

            // Check for abbreviated second chapter
            if (result.FirstVerseString == null && result.SecondVerseString == null && result.FirstBook == result.SecondBook && result.SecondChapter < result.FirstChapter && result.SecondChapter.ToString().Length < result.FirstChapter.ToString().Length)
            {
                result.SecondChapter = int.Parse( result.FirstChapter.ToString().Substring(0, result.FirstChapter.ToString().Length - result.SecondChapter.ToString().Length) + result.SecondChapter.ToString());
            }

            // Check for abbreviated second verse
            if (result.FirstBook == result.SecondBook && result.FirstChapterString != null && result.FirstChapter == result.SecondChapter && result.SecondVerse < result.FirstVerse && result.SecondVerse.ToString().Length < result.FirstVerse.ToString().Length)
            {
                result.SecondVerse = int.Parse(result.FirstVerse.ToString().Substring(0, result.FirstVerse.ToString().Length - result.SecondVerse.ToString().Length) + result.SecondVerse.ToString());
            }

            return result;
        } 
        #endregion

        #region internal Properties

        internal bool IsFirstBookExplicit { get; set; }

        /// <summary>
        /// Gets or sets the first book.
        /// </summary>
        /// <value>
        /// The first book.
        /// </value>
        internal string FirstBook { get; set; }

        #region FirstChapter
        /// <summary>
        /// Gets or sets the first chapter as a string.
        /// </summary>
        /// <value>
        /// The first chapter.
        /// </value>
        internal string FirstChapterString
        {
            get
            {
                return _firstChapterString;
            }
            set
            {
                _firstChapterString = value;
                _firstChapter = -1;
                int.TryParse(value, out _firstChapter);
            }
        }
        internal string _firstChapterString; 

        /// <summary>
        /// Gets or sets the first chapter as a number.
        /// </summary>
        /// <value>
        /// The first chapter.
        /// </value>
        internal int FirstChapter
        {
            get
            {
                return _firstChapter;
            }
            set
            {
                _firstChapter = value;
                _firstChapterString = value == -1 ? null : value.ToString();
            }
        }
        private int _firstChapter = -1; 
        #endregion

        #region FirstVerse
        /// <summary>
        /// Gets or sets the first verse as a string.
        /// </summary>
        /// <value>
        /// The first verse.
        /// </value>
        internal string FirstVerseString
        {
            get
            {
                return _firstVerseString;
            }
            set
            {
                _firstVerseString = value;

                if (value == null)
                {
                    _firstVerse = -1;
                    _firstVerseSuffix = null;
                }
                else
                {
                    ParseVerseString(value, out _firstVerse, out _firstVerseSuffix);
                }
            }
        }
        private string _firstVerseString;

        /// <summary>
        /// Gets or sets the first verse as a number.
        /// </summary>
        /// <value>
        /// The first verse.
        /// </value>
        internal int FirstVerse
        {
            get
            {
                return _firstVerse;
            }
            set
            {
                _firstVerse = value;
                _firstVerseString = value.ToString() + _firstVerseSuffix;
            }
        }
        private int _firstVerse = -1;

        /// <summary>
        /// Gets or sets an optional first verse suffix.
        /// </summary>
        /// <value>
        /// The first verse suffix.
        /// </value>
        internal string FirstVerseSuffix
        {
            get
            {
                return _firstVerseSuffix;
            }
            set
            {
                _firstVerseSuffix = value;
                if(_firstVerseString != null)
                    _firstVerseString = _firstVerse.ToString() + value;
            }
        }
        private string _firstVerseSuffix; 
        #endregion

        /// <summary>
        /// Gets or sets the second book.
        /// </summary>
        /// <value>
        /// The second book.
        /// </value>
        internal string SecondBook { get; set; }

        #region SecondChapter
        /// <summary>
        /// Gets or sets the second chapter.
        /// </summary>
        /// <value>
        /// The second chapter.
        /// </value>
        internal string SecondChapterString
        {
            get
            {
                return _secondChapterString;
            }
            set
            {
                _secondChapterString = value;
                _secondChapter = -1;
                int.TryParse(value, out _secondChapter);
            }
        }
        private string _secondChapterString;

        internal int SecondChapter
        {
            get
            {
                return _secondChapter;
            }
            set
            {
                _secondChapter = value;
                _secondChapterString = value == -1 ? null : value.ToString();
            }
        }
        private int _secondChapter = -1; 
        #endregion

        #region SecondVerse
        /// <summary>
        /// Gets or sets the second verse as a string.
        /// </summary>
        /// <value>
        /// The second verse.
        /// </value>
        internal string SecondVerseString
        {
            get
            {
                return _secondVerseString;
            }
            set
            {
                _secondVerseString = value;

                if (value == null)
                {
                    _secondVerse = -1;
                    _secondVerseSuffix = null;
                }
                else
                {
                    ParseVerseString(value, out _secondVerse, out _secondVerseSuffix);
                }
            }
        }
        private string _secondVerseString;

        /// <summary>
        /// Gets or sets the second verse as a number.
        /// </summary>
        /// <value>
        /// The second verse.
        /// </value>
        internal int SecondVerse
        {
            get
            {
                return _secondVerse;
            }
            set
            {
                _secondVerse = value;
                _secondVerseString = value.ToString() + _secondVerseSuffix;
            }
        }
        private int _secondVerse = -1;

        /// <summary>
        /// Gets or sets an optional second verse suffix.
        /// </summary>
        /// <value>
        /// The second verse suffix.
        /// </value>
        internal string SecondVerseSuffix
        {
            get
            {
                return _secondVerseSuffix;
            }
            set
            {
                _secondVerseSuffix = value;
                if(_secondVerseString != null)
                _secondVerseString = _secondVerse.ToString() + value;
            }
        }
        private string _secondVerseSuffix; 
        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses the verse string into a verse number and optional suffix.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="verse">The verse.</param>
        /// <param name="suffix">The suffix.</param>
        private static void ParseVerseString(string input, out int verse, out string suffix)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                verse = -1;
                suffix = null;
            }
            else
            {

                input = input.Trim().ToLower();

                if (input.EndsWith("a") || input.EndsWith("b") || input.EndsWith("c"))
                {
                    if (input.Length > 1)
                    {
                        verse = -1;
                        if (!int.TryParse(input.Substring(0, input.Length - 1), out verse))
                            throw new FormatException("s is not in the correct format.");
                        suffix = input.Substring(input.Length - 1, 1);
                    }
                    else
                    {
                        throw new FormatException("s is not in the correct format.");
                    }
                }
                else
                {
                    verse = -1;
                    if (!int.TryParse(input, out verse))
                        throw new FormatException("s is not in the correct format.");
                    suffix = null;
                }
            }
        }

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
            int result = 0;

            if (this.FirstBook != null)
                result ^= (this.FirstBook.ToLower().Trim().GetHashCode() >> 1);

            if (this.FirstChapterString != null)
                result ^= (this.FirstChapterString.ToLower().Trim().GetHashCode() >> 1);

            if (this.FirstChapter != -1)
                result ^= (this.FirstChapter << 1);

            if (this.FirstVerseString != null)
                result ^= (this.FirstVerseString.ToLower().Trim().GetHashCode() << 1);

            if (this.FirstVerse != -1)
                result ^= (this.FirstVerse << 3);

            if (this.FirstVerseSuffix != null)
                result ^= (this.FirstVerseSuffix.ToLower().Trim().GetHashCode() << 1);

            if (this.SecondBook != null)
                result ^= (this.SecondBook.ToLower().Trim().GetHashCode() << 2);

            if (this.SecondChapterString != null)
                result ^= (this.SecondChapterString.ToLower().Trim().GetHashCode() >> 2);

            if (this.SecondChapter != -1)
                result ^= (this.SecondChapter >> 1);

            if (this.SecondVerseString != null)
                result ^= (this.SecondVerseString.ToLower().Trim().GetHashCode() << 2);

            if (this.SecondVerse != -1)
                result ^= (this.SecondVerse >> 3);

            if (this.SecondVerseSuffix != null)
                result ^= (this.SecondVerseSuffix.ToLower().Trim().GetHashCode() >> 1);

            return result;
        } 
        #endregion
    }
}
