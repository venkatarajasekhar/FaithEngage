using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Konves.Scripture
{
    internal static class Int
    {
        public static bool Between(this int input, int start, int end)
        {
            return input >= start && input <= end;
        }
    }
}

namespace Konves.Scripture.Version
{
    public class ScriptureInfo
    {
        private ScriptureInfo() { }

        /// <summary>dictionary for storing registered instances and paths</summary>
        private static volatile Dictionary<string, string> paths;
        private static volatile Dictionary<string, Version.ScriptureInfo> instances;
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// Registers the specified Scripture ID and path of a file defining the scripture info.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// Returns <c>true</c> if the ID and path were registered successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryRegister(string id, string path)
        {
            lock (syncRoot)
            {
                if (paths == null)
                    paths = new Dictionary<string, string>();

                if (paths.ContainsKey((id = id.ToLower().Trim())))
                {
                    return false;
                }
                else
                {
                    paths.Add(id, path);
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets an instance of a <see cref="ScriptureReference"/> object for a scripture ID or <c>null</c> if the ID is not registered or the file is not found or in an incorrect format.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Returns an instance of a <see cref="ScriptureReference"/> object or <c>null</c>.
        /// </returns>
        public static Version.ScriptureInfo GetInstance(string id)
        {
            lock (syncRoot)
            {
                if (paths != null && paths.ContainsKey((id = id.ToLower().Trim())))
                {
                    if (instances == null)
                        instances = new Dictionary<string, Version.ScriptureInfo>();

                    if (!instances.ContainsKey(id))
                    {
                        // TODO: consider handling exception thrown during load
                        var result = LoadScriptureInfo(paths[id]);

                        if (result == null)
                            return null;
                        else
                            instances.Add(id, result);
                    }

                    return instances[id];
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Loads the file at the specified <paramref name="path"/> and then deserializes it into a <see cref="Version.ScriptureInfo"/> object.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>
        /// Returns a <see cref="Version.ScriptureInfo"/> object.
        /// </returns>
        private static Version.ScriptureInfo LoadScriptureInfo(string path)
        {
            Version.ScriptureInfo si = null;

            Stream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Open);
                si = Deserialize(stream);
            }
            catch (Exception)
            {
                // Exceptions result in null resource.
            }
            finally
            {
                if (stream != null) ((IDisposable)stream).Dispose();
            }
            return si;
        }

        public static Version.ScriptureInfo Deserialize(Stream stream)
        {
            Version.ScriptureInfo result = new Version.ScriptureInfo();

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(stream))
            {
                while (r.Read())
                {
                    if (r.Name == "Resource")
                    {

                        result.Name = r.GetAttribute("name");
                        result.TranslationName = r.GetAttribute("tname");
                        result.TranslationAbbreviation = r.GetAttribute("tabbr");



                        while (r.Read())
                        {
                            if (r.Name == "Limits")
                            {
                                r.Read();
                                var limits = new List<ChapterLimits>();

                                while (r.ReadToNextSibling("L"))
                                {
                                    limits.Add(new ChapterLimits
                                    {
                                        BookNumber = int.Parse(r.GetAttribute("b")),
                                        ChapterNumber = int.Parse(r.GetAttribute("c")),
                                        StartVerseIndex = int.Parse(r.GetAttribute("s")),
                                        EndVerseIndex = int.Parse(r.GetAttribute("e")),
                                        EndVerseNumber = int.Parse(r.GetAttribute("v"))
                                    });
                                }
                                result.ChapterLimits = limits.ToArray();
                            }
                            else if (r.Name == "Books")
                            {
                                r.Read();

                                var books = new List<ScriptureBook>();

                                while (r.ReadToNextSibling("B"))
                                {

                                    string o = r.GetAttribute("o");

                                    var book = new ScriptureBook
                                    {
                                        Name = r.GetAttribute("n"),
                                        Number = int.Parse(r.GetAttribute("i")),
                                        ShortAbbr = r.GetAttribute("s"),
                                        LongAbbr = r.GetAttribute("l"),
                                        Ordinal = string.IsNullOrEmpty(o) ? -1 : int.Parse(o)
                                    };

                                    var abbrs = new List<string>();

                                    r.Read();

                                    while (r.ReadToNextSibling("A"))
                                    {
                                        abbrs.Add(r.ReadInnerXml());
                                    }
                                    book.Abbreviations = abbrs.ToArray();

                                    books.Add(book);
                                }
                                result.Books = books.ToArray();
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the name of the scripture.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name of the scripture translation.
        /// </summary>
        /// <value>
        /// The name of the translation.
        /// </value>
        public string TranslationName { get; private set; }

        /// <summary>
        /// Gets the abbreviation for the translation of this scripture.
        /// </summary>
        public string TranslationAbbreviation { get; private set; }

        /// <summary>
        /// Gets a collection of <see cref="ChapterLimits"/> objects which define the limits of each chapter in this scripture.
        /// </summary>
        public ChapterLimits[] ChapterLimits { get; private set; }

        /// <summary>
        /// Gets or sets the information about the books in this scripture.
        /// </summary>
        /// <value>
        /// The books.
        /// </value>
        private ScriptureBook[] Books { get; set; }

        private int[] VerseOmissions { get; set; }

        internal IEnumerable<Version.ChapterLimits> GetChapterLimits(int startBook, int startChapter, int endBook, int endChapter)
        {
            if (startBook == endBook)
                return
                from limit in this.ChapterLimits
                where
                    limit.BookNumber == startBook
                    && limit.ChapterNumber.Between(startChapter, endChapter)
                select limit;
            else
                return
                    from limit in this.ChapterLimits
                    where
                        (limit.BookNumber == startBook && limit.ChapterNumber >= startChapter)
                        || (limit.BookNumber == endBook && limit.ChapterNumber <= endChapter)
                        || (limit.BookNumber > startBook && limit.BookNumber < endBook)
                    select limit;
        }

        /// <summary>
        /// Gets the one-base number of the book specified by abbreviation.
        /// </summary>
        /// <param name="abbr">The book abbreviation (not case-sensitive).</param>
        /// <returns>
        /// Returns a 32-bit signed-integer value representing the one-based book number.
        /// </returns>
        public int GetBookNumber(string abbr)
        {
            var book = this.Books.FirstOrDefault(b => b.Abbreviations.Select(a => a.ToLower()).Contains(abbr.Trim().ToLower()));

            if (book == null)
                throw new ArgumentOutOfRangeException("abbr");
            else
                return book.Number;
        }

        /// <summary>
        /// Gets the validated name of the book by its abbreviation or name.
        /// </summary>
        /// <param name="abbr">The book abbreviation or name (not case-sensitive).</param>
        /// <returns>
        /// Returns the name of the book.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="abbr"/> does not represent a valid book.</exception>
        public string GetBookName(string abbr)
        {
            // TODO: Edit for clarity
            var book = this.Books.FirstOrDefault(b => b.Abbreviations.Select(a => a.ToLower()).Concat(new string[] { b.Name.ToLower() }).Contains(abbr.Trim().ToLower()));

            if (book == null)
                // TODO: return null instead of throwing exception
                throw new ArgumentOutOfRangeException("abbr", "abbr does not represent a valid book.");
            else
                return book.Name;
        }

        /// <summary>
        /// Gets the number of the last chapter of the book specified by number.
        /// </summary>
        /// <param name="bookNumber">The one-based book number.</param>
        /// <returns>
        /// Returns a 32-bit signed-integer value representing the one-based chapter number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="bookNumber"/> does not represent a valid book.</exception>
        internal int GetLastChapter(int bookNumber)
        {
            var limits = this.ChapterLimits.Where(l => l.BookNumber == bookNumber);

            if (limits == null || !limits.Any())
                // TODO: return null instead of throwing exception
                throw new ArgumentOutOfRangeException("bookId", "bookId does not represent a valid book.");
            else
                return limits.Max(l => l.ChapterNumber);
        }

        /// <summary>
        /// Gets the last verse of the specified book and chapter.
        /// </summary>
        /// <param name="bookNumber">The one-based book number.</param>
        /// <param name="bookNumber">The one-based chapter number.</param>
        /// <returns>
        /// Returns a 32-bit signed-integer value representing the one-based chapter number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="bookNumber"/> and <paramref name="chapter"/> do not represent a valid book and chapter.</exception>
        internal int GetLastVerse(int bookNumber, int chapter)
        {
            var limits = this.ChapterLimits.FirstOrDefault(l => l.BookNumber == bookNumber && l.ChapterNumber == chapter);

            if (limits == null)
                throw new ArgumentOutOfRangeException();
            else
                return limits.EndVerseNumber;
        }

        /// <summary>
        /// Gets the validated verse data corresponding to the specified book number, chapter and verse.
        /// </summary>
        /// <param name="bookId">The one-based book number.</param>
        /// <param name="chapter">The one-based chapter number.</param>
        /// <param name="verse">The one-based verse.</param>
        /// <param name="index">The validated index, or -1 if the book number, chapter and verse do not validate.</param>
        /// <param name="chapterResult">The validated, one-based chapter number, or -1 if the book number, chapter and verse do not validate.</param>
        /// <param name="verseResult">The validated, one-based verse number, or -1 if the book number, chapter and verse do not validate.</param>
        internal void GetVerseData(int bookNumber, int chapter, int verse, out int index, out int chapterResult, out int verseResult)
        {
            var limits = this.ChapterLimits.FirstOrDefault(l => l.BookNumber == bookNumber && l.ChapterNumber == chapter);

            if (limits == null)
            {
                index = -1;
                chapterResult = -1;
                verseResult = -1;
            }
            else
            {
                index = limits.StartVerseIndex + verse;
                chapterResult = limits.ChapterNumber;
                verseResult = verse;
            }
        }

        /// <summary>
        /// Gets the book info.
        /// </summary>
        /// <param name="abbr">The abbr.</param>
        /// <returns></returns>
        internal Data.BookInfo GetBookInfo(string abbr)
        {
            // TODO: Edit for clarity
            var x = this.Books.FirstOrDefault(b => b.Abbreviations.Select(a => a.ToLower()).Concat(new string[] { b.Name.ToLower() }).Contains(abbr.Trim().ToLower()));

            if (x == null)
                return null;
            else
                return new Data.BookInfo
                {
                    Number = x.Number,
                    Name = x.Name,
                    LongAbbr = x.LongAbbr,
                    ShortAbbr = x.ShortAbbr,
                    Ordinal = x.Ordinal
                };
        }

        internal Data.BookInfo GetBookInfo(int bookId)
        {
            var x = this.Books.FirstOrDefault(b => b.Number == bookId);

            if (x == null)
                return null;
            else
                return new Data.BookInfo
                {
                    Number = x.Number,
                    Name = x.Name,
                    LongAbbr = x.LongAbbr,
                    ShortAbbr = x.ShortAbbr,
                    Ordinal = x.Ordinal
                };
        }

        [System.Diagnostics.DebuggerDisplay("{Number}: {Name}")]
        private class ScriptureBook
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public string ShortAbbr { get; set; }
            public string LongAbbr { get; set; }
            public int Ordinal { get; set; }

            public string[] Abbreviations { get; set; }
        }
    }

    public class ChapterLimits
    {
        public int BookNumber { get; set; }
        public int ChapterNumber { get; set; }
        public int StartVerseIndex { get; set; }
        public int EndVerseIndex { get; set; }
        public int EndVerseNumber { get; set; }
    }
}