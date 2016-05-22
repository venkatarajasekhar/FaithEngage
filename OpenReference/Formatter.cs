using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Scripture
{
    public class DefaultFormatProvider : IFormatProvider
    {
        private DefaultFormatProvider() { }

        private static volatile IFormatProvider instance;
        private static readonly object syncRoot = new object();

        public static IFormatProvider Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new DefaultFormatProvider());
                }
            }
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(Reference))
            {
                return ReferenceFormatter.Instance;
            }
            else if (formatType == typeof(Range))
            {
                return RangeFormatter.Instance;
            }
            else if (formatType == typeof(Verse))
            {
                return VerseFormatter.Instance;
            }
            else
                return null;
        }
    }

    /// <summary>
    /// Provides formatting services for the <see cref="Reference"/> class.
    /// </summary>
    public sealed class ReferenceFormatter : ICustomFormatter
    {
        #region "Singleton"
        private ReferenceFormatter() { }

        private static volatile ICustomFormatter instance;
        private static readonly object syncRoot = new object();

        public static ICustomFormatter Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new ReferenceFormatter());
                }
            }
        } 
        #endregion
        
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // [BBB [c:[vv-],-],-];

            List<string> rangeStrings = new List<string>();

            Reference reference = (Reference)arg;

            string result = string.Empty;

            int currentBook = -1;
            int currentChapter = -1;
            int currentVerse = -1;

            IEnumerable<Reference> refs = reference.HasSubReferences ? reference.GetSubReferences() : new Reference[] { reference };

            foreach (Reference r in reference.GetSubReferences())
            {
                if (r.FirstVerse.BookNumber == currentBook && r.FirstVerse.ChapterNumber == currentChapter)
                {
                    if (r.LastVerse.BookNumber == r.FirstVerse.BookNumber && r.LastVerse.ChapterNumber == r.FirstVerse.ChapterNumber && r.LastVerse.VerseNumber == r.FirstVerse.VerseNumber)
                    {
                        // See test 2
                        result = r.LastVerse.VerseNumber.ToString();
                    }
                    else if (r.LastVerse.BookNumber == r.FirstVerse.BookNumber && r.LastVerse.ChapterNumber == r.FirstVerse.ChapterNumber)
                    {
                        // See test 3
                        result = r.FirstVerse.VerseNumber.ToString() + "-" + r.LastVerse.VerseNumber.ToString();
                    }
                    else if (r.LastVerse.BookNumber == r.FirstVerse.BookNumber)
                    {
                        // See test 6
                        result = r.FirstVerse.VerseNumber.ToString() + "-" + r.LastVerse.ChapterNumber.ToString() + ":" + r.LastVerse.VerseNumber.ToString();
                    }
                    else
                    {
                        // See test 7
                        result = r.FirstVerse.VerseNumber.ToString() + " - " + r.LastVerse.ToString();
                    }
                }
                else if (r.FirstVerse.BookNumber == currentBook)
                {
                    if (r.LastVerse.BookNumber == r.FirstVerse.BookNumber && r.LastVerse.ChapterNumber == r.FirstVerse.ChapterNumber && r.LastVerse.VerseNumber == r.FirstVerse.VerseNumber)
                    {
                        // See test 4
                        result = r.FirstVerse.ChapterNumber.ToString() + ":" + r.FirstVerse.VerseNumber.ToString();
                    }
                    else if (r.LastVerse.BookNumber == r.FirstVerse.BookNumber && r.LastVerse.ChapterNumber == r.FirstVerse.ChapterNumber)
                    {
                        // See test 5
                        result = r.FirstVerse.ChapterNumber.ToString() + ":" + r.FirstVerse.VerseNumber.ToString() + "-" + r.LastVerse.VerseNumber.ToString();
                    }
                    else if (r.LastVerse.BookNumber == r.FirstVerse.BookNumber)
                    {
                        // See test 8
                        result = r.FirstVerse.ChapterNumber.ToString() + ":" + r.FirstVerse.VerseNumber.ToString() + "-" + r.LastVerse.ChapterNumber.ToString() + ":" + r.LastVerse.VerseNumber.ToString();
                    }
                    else
                    {
                        // See test 9
                        result = r.FirstVerse.ChapterNumber.ToString() + ":" + r.FirstVerse.VerseNumber.ToString() + " - " + r.LastVerse.ToString();
                    }
                }
                else
                {
                    // See test 1
                    result = r.ToString();
                }

                if (currentBook != -1)
                {
                    if (r.FirstVerse.BookNumber != currentBook)
                        result = "; " + result;
                    else
                        result = "," + result;
                }

                rangeStrings.Add(result);

                currentBook = r.LastVerse.BookNumber;
                currentChapter = r.LastVerse.ChapterNumber;
                currentVerse = r.LastVerse.VerseNumber;
            }

            return string.Join("", rangeStrings);
        }
    }

    /// <summary>
    /// Provides formatting services for the <see cref="Range"/> class.
    /// </summary>
    public sealed class RangeFormatter : ICustomFormatter
    {
        #region "Singleton"
        private RangeFormatter() { }

        private static volatile ICustomFormatter instance;
        private static readonly object syncRoot = new object();

        public static ICustomFormatter Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new RangeFormatter());
                }
            }
        }
        #endregion

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // TODO: add funtionality to create short strings: "Genesis 1-6" or "Matthew-John"

            string verseFormat = "BBB c:v";

            Range r = arg as Range;

            if (r.Start != null && r.End != null)
            {
                if (r.Start.BookNumber == r.End.BookNumber && r.Start.ChapterNumber == r.End.ChapterNumber && r.Start.VerseNumber == r.End.VerseNumber)
                {
                    return r.Start.ToString(verseFormat, formatProvider);
                }
                else if (r.Start.BookNumber == r.End.BookNumber && r.Start.ChapterNumber == r.End.ChapterNumber)
                {
                    return r.Start.BookName + " " + r.Start.ChapterNumber + ":" + r.Start.VerseNumber + r.Start.Suffix + "-" + r.End.VerseNumber + r.End.Suffix;
                }
                else if (r.Start.BookNumber == r.End.BookNumber)
                {
                    return r.Start.BookName + " " + r.Start.ChapterNumber + ":" + r.Start.VerseNumber + r.Start.Suffix + "-" + r.End.ChapterNumber + ":" + r.End.VerseNumber + r.End.Suffix;
                }
                else
                    return r.Start.ToString(verseFormat, formatProvider) + " - " + r.End.ToString(verseFormat, formatProvider);
            }
            else
                return base.ToString();
        }
    }


    /// <summary>
    /// Provides formatting services for the <see cref="Verse"/> class.
    /// </summary>
    public sealed class VerseFormatter : ICustomFormatter
    {
        #region "Singleton"
        private VerseFormatter() { }

        private static volatile ICustomFormatter instance;
        private static readonly object syncRoot = new object();
        private const string _defaultFormat = "BBB c:vs (t)";

        public static ICustomFormatter Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new VerseFormatter());
                }
            }
        } 
        #endregion

        public static string DefaultFormat
        {
            get
            {
                return _defaultFormat;
            }
        }

        /// <summary>
        /// Converts the value of a specified object to an equivalent string representation using specified format and culture-specific formatting information.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.</param>
        /// <param name="arg">An object to format.</param>
        /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
        /// <returns>
        /// The string representation of the value of <paramref name="arg"/>, formatted as specified by <paramref name="format"/> and <paramref name="formatProvider"/>.
        /// </returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();

            Verse verse = (Verse)arg;

            if (format == null)
                format = DefaultFormat;

            for (int i = 0; i < format.Length; i++)
            {
                if (i < format.Length && format[i] == '\\')
                {
                    sb.Append(format[i + 1]);
                    i += 1; // charater is escaped
                }
                else if (i + 2 < format.Length && format[i] == 'B' && format[i + 1] == 'B' && format[i + 2] == 'B') // BBB - Capitalized name of book
                {
                    sb.Append(verse.BookName);
                    i += 2;
                }
                else if (i + 1 < format.Length && format[i] == 'B' && format[i + 1] == 'B') // BB - Capitalized long abbreviation of book
                {
                    sb.Append(verse.LongBookAbbreviation);
                    i += 1;
                }
                else if (format[i] == 'B') // B - Capitalized short abbreviation of book
                {
                    sb.Append(verse.ShortBookAbbreviation);
                }
                else if (i + 2 < format.Length && format[i] == 'b' && format[i + 1] == 'b' && format[i + 2] == 'b') // bbb - Lower case name of book
                {
                    sb.Append(verse.BookName.ToLower());
                    i += 2;
                }
                else if (i + 1 < format.Length && format[i] == 'b' && format[i + 1] == 'b') // bb - Lower case long abbreviation of book
                {
                    sb.Append(verse.LongBookAbbreviation.ToLower()); 
                    i += 1;
                }
                else if (format[i] == 'b') // b - Lower case short abbreviation of book
                {
                    sb.Append(verse.ShortBookAbbreviation.ToLower());
                }
                else if (format[i] == 'c') // c - Chapter number
                {
                    sb.Append(verse.ChapterNumber);
                }
                else if (format[i] == 'v') // v - Verse number
                {
                    sb.Append(verse.VerseNumber);
                }
                else if (format[i] == 's') // s - Verse suffix
                {
                    sb.Append(verse.Suffix);
                }
                else if (format[i] == 'n') // n - Name of scripture
                {
                    sb.Append(verse.ScriptureName);
                }
                else if (i + 1 < format.Length && format[i] == 't' && format[i + 1] == 't') // tt - Name of scripture translation
                {
                    sb.Append(verse.TranslationName);
                    i += 1;
                }
                else if (format[i] == 't') // t - Lowercase translation of scripture translation
                {
                    sb.Append(verse.TranslationAbbreviation.ToLower());
                }
                else if (format[i] == 'T') // T - Uppercase translation of scripture translation
                {
                    sb.Append(verse.TranslationAbbreviation.ToUpper());
                }
                else
                {
                    sb.Append(format[i]);
                }
            }

            return sb.ToString();
        }
    }
}
