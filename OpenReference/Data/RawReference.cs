using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Scripture.Data
{
    internal class RawReference : List<RawRange>
    {
        /// <summary>
        /// Converts the string representation of a full scripture range to its RawReference equivalent.
        /// </summary>
        /// <param name="s">A string containing a full scripture range to convert.</param>
        /// <returns>
        /// A RawReference equivalent to the full scripture range contained in <paramref name="s"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="s"/> is null.</exception>
        internal static RawReference Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException("s is null");

            RawReference result = new RawReference();

            string[] segs = input.Split(new char[]{';', ','},StringSplitOptions.RemoveEmptyEntries);

            string currentBook = null;
            string currentChapter = null;
            string currentVerse = null;

            foreach (string seg in segs)
            {
                RawRange range = RawRange.Parse(seg, currentBook, currentChapter, currentVerse);

                if (currentBook == null)
                {
                    currentBook = range.SecondBook;
                    currentChapter = range.SecondChapterString;
                    currentVerse = range.SecondVerseString;
                }
                else if (currentBook != range.SecondBook)
                {
                    currentBook = range.SecondBook;
                    currentChapter = null;
                    currentVerse = null;
                }
                else if (currentChapter != range.SecondChapterString)
                {
                    currentChapter = range.SecondChapterString;
                    currentVerse = null;
                }
                else if (currentVerse != range.SecondVerseString)
                {
                    currentVerse = range.SecondVerseString;
                }

                // check for abbreviated numbers
                if (!range.IsFirstBookExplicit && result.Any())
                {
                    RawRange last = result.Last();

                    // Check for abbreviated chapters
                    if (last.FirstVerseString == null
                        && last.SecondVerseString == null
                        && range.FirstVerseString == null
                        && range.SecondVerseString == null
                        && range.FirstBook == last.SecondBook)
                    {
                        // Check for abbreviated first chapter
                        if (range.FirstChapter < last.SecondChapter
                            && range.FirstChapterString.Length < last.SecondChapterString.Length)
                            {
                                range.FirstChapterString = last.SecondChapterString.Substring(0, last.SecondChapterString.Length - range.FirstChapterString.Length) + range.FirstChapterString;
                            }

                        // Check for abbreviated second chapter
                        if (range.SecondChapter < last.SecondChapter
                            && range.SecondChapterString.Length < last.SecondChapterString.Length)
                        {
                            range.SecondChapterString = last.SecondChapterString.Substring(0, last.SecondChapterString.Length - range.SecondChapterString.Length) + range.SecondChapterString;
                        }
                    }

                    // check for abbreviated first verse
                    if (last.SecondBook == range.FirstBook 
                        && last.SecondChapterString != null 
                        && last.SecondChapter == range.FirstChapter

                        && range.FirstVerse < last.SecondVerse
                        && range.FirstVerse.ToString().Length < last.SecondVerse.ToString().Length)
                    {
                        range.FirstVerse = int.Parse(last.SecondVerse.ToString().Substring(0, last.SecondVerse.ToString().Length - range.FirstVerse.ToString().Length) + range.FirstVerse.ToString());
                    }

                    // check for abbreviated second verse
                    if (last.SecondBook == range.FirstBook
                        && last.SecondChapterString != null
                        && last.SecondChapter == range.FirstChapter

                        && range.SecondVerse < last.SecondVerse
                        && range.SecondVerse.ToString().Length < last.SecondVerse.ToString().Length)
                    {
                        range.SecondVerse = int.Parse(last.SecondVerse.ToString().Substring(0, last.SecondVerse.ToString().Length - range.SecondVerse.ToString().Length) + range.SecondVerse.ToString());
                    }

                }

                result.Add(range);
            }

            return result;
        }

        // TODO: implement tryparse
    }
}
