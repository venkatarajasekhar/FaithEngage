using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konves.Scripture.Data;

namespace Konves.Scripture
{
    ///// <summary>
    ///// Represents a collection of one or more ranges of scripture verses.
    ///// </summary>
    //internal class RangeCollection_OBE : List<Range>
    //{
    //    /// <summary>
    //    /// Converts the string representation of a scripture range to its <see cref="RangeCollection_OBE"/> equivalent.
    //    /// </summary>
    //    /// <param name="range">A string containing a scripture range to convert.</param>
    //    /// <returns>
    //    /// A <see cref="RangeCollection_OBE"/> equivalent to the scripture range contained in <paramref name="range"/>.
    //    /// </returns>
    //    /// <exception cref="System.ArgumentNullException">
    //    ///     <paramref name="range"/> is null.</exception>
    //    /// <exception cref="System.ArgumentException">
    //    ///     <paramref name="range"/> does not contain any text.</exception>
    //    public static RangeCollection_OBE Parse(string reference, Version.ScriptureInfo scriptureInfo)
    //    {
    //        return Parse(reference, scriptureInfo, RepositoryMode.Safe);
    //    }

    //    /// <summary>
    //    /// Converts the string representation of a scripture range to its <see cref="RangeCollection_OBE"/> equivalent.
    //    /// </summary>
    //    /// <param name="s">A string containing a scripture range to convert.</param>
    //    /// <param name="mode">The mode.</param>
    //    /// <returns>
    //    /// A <see cref="RangeCollection_OBE"/> equivalent to the scripture range contained in <paramref name="s"/>.
    //    /// </returns>
    //    /// <exception cref="System.ArgumentNullException">
    //    ///   <paramref name="s"/> is null.</exception>
    //    ///   
    //    /// <exception cref="System.ArgumentException">
    //    ///   <paramref name="s"/> does not contain any text.</exception>
    //    public static RangeCollection_OBE Parse(string input, Version.ScriptureInfo scriptureInfo, RepositoryMode mode)
    //    {
    //        if (input == null)
    //            throw new ArgumentNullException("s", "range is null");

    //        if (string.IsNullOrWhiteSpace(input))
    //            throw new ArgumentException("s does not contain any text", "s");

    //        RangeCollection_OBE result = new RangeCollection_OBE();

    //        if (TryParse(input, scriptureInfo, mode, out result))
    //            return result;
    //        else
    //            throw new ReferenceFormatException();
    //    }

    //    public static bool TryParse(string input, Version.ScriptureInfo scriptureInfo, out RangeCollection_OBE reference)
    //    {
    //        return TryParse(input, scriptureInfo, RepositoryMode.Safe, out reference);
    //    }

    //    public static bool TryParse(string s, Version.ScriptureInfo scriptureInfo, RepositoryMode mode, out RangeCollection_OBE reference)
    //    {
    //        if (s == null)
    //        {
    //            reference = null;
    //            return false;
    //        }

    //        if (string.IsNullOrWhiteSpace(s))
    //        {
    //            reference = null;
    //            return false;
    //        }
            
    //        reference = new RangeCollection_OBE();

    //        RawReference rawRef = RawReference.Parse(s);

    //        foreach (RawRange range in rawRef)
    //        {
    //            var x = Range.Create(scriptureInfo, range,0, mode);

    //            if (x == null)
    //            {
    //                if (mode == RepositoryMode.Strict)
    //                {
    //                    reference = null;
    //                    return false;
    //                }
    //            }                    
    //            else
    //                reference.Add(x);
    //        }

    //        return true;
    //    }


    //    ///// <summary>
    //    ///// Returns a <see cref="System.String"/> that represents this instances.
    //    ///// </summary>
    //    ///// <returns>
    //    ///// A <see cref="System.String"/> that represents this instances.
    //    ///// </returns>
    //    //public override string ToString()
    //    //{
    //    //    return "NotImplementedException";
    //    //    throw new NotImplementedException();

    //    //    List<string> rangeStrings = new List<string>();

    //    //    string result = string.Empty;

    //    //    int currentBook = -1;
    //    //    int currentChapter = -1;
    //    //    int currentVerse = -1;

    //    //    string verseFormat = "BBB c:v";
    //    //    IFormatProvider formatProvider = DefaultFormatProvider.Instance;

    //    //    foreach (Range range in this)
    //    //    {
    //    //        if (range.Start.BookNumber == currentBook && range.Start.ChapterNumber == currentChapter)
    //    //        {
    //    //            if (range.End.BookNumber == range.Start.BookNumber && range.End.ChapterNumber == range.Start.ChapterNumber && range.End.VerseNumber == range.Start.VerseNumber)
    //    //            {
    //    //                // See test 2
    //    //                result = range.End.VerseNumber.ToString();
    //    //            }
    //    //            else if (range.End.BookNumber == range.Start.BookNumber && range.End.ChapterNumber == range.Start.ChapterNumber)
    //    //            {
    //    //                // See test 3
    //    //                result = range.Start.VerseNumber.ToString() + "-" + range.End.VerseNumber.ToString();
    //    //            }
    //    //            else if (range.End.BookNumber == range.Start.BookNumber)
    //    //            {
    //    //                // See test 6
    //    //                result = range.Start.VerseNumber.ToString() + "-" + range.End.ChapterNumber.ToString() + ":" + range.End.VerseNumber.ToString();
    //    //            }
    //    //            else
    //    //            {
    //    //                // See test 7
    //    //                result = range.Start.VerseNumber.ToString() + " - " + range.End.ToString(verseFormat, formatProvider);
    //    //            }
    //    //        }
    //    //        else if (range.Start.BookNumber == currentBook)
    //    //        {
    //    //            if (range.End.BookNumber == range.Start.BookNumber && range.End.ChapterNumber == range.Start.ChapterNumber && range.End.VerseNumber == range.Start.VerseNumber)
    //    //            {
    //    //                // See test 4
    //    //                result = range.Start.ChapterNumber.ToString() + ":" + range.Start.VerseNumber.ToString();                    
    //    //            }
    //    //            else if (range.End.BookNumber == range.Start.BookNumber && range.End.ChapterNumber == range.Start.ChapterNumber)
    //    //            {
    //    //                // See test 5
    //    //                result = range.Start.ChapterNumber.ToString() + ":" + range.Start.VerseNumber.ToString() + "-" + range.End.VerseNumber.ToString();
    //    //            }
    //    //            else if (range.End.BookNumber == range.Start.BookNumber)
    //    //            {
    //    //                // See test 8
    //    //                result = range.Start.ChapterNumber.ToString() + ":" + range.Start.VerseNumber.ToString() + "-" + range.End.ChapterNumber.ToString() + ":" + range.End.VerseNumber.ToString();
    //    //            }
    //    //            else
    //    //            {
    //    //                // See test 9
    //    //                result = range.Start.ChapterNumber.ToString() + ":" + range.Start.VerseNumber.ToString() + " - " + range.End.ToString(verseFormat, formatProvider);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            // See test 1
    //    //            result = range.ToString();
    //    //        }

    //    //        if (currentBook != -1)
    //    //        {
    //    //            if (range.Start.BookNumber != currentBook)
    //    //                result = "; " + result;
    //    //            else
    //    //                result = "," + result;
    //    //        }

    //    //        rangeStrings.Add(result);

    //    //        currentBook = range.End.BookNumber;
    //    //        currentChapter = range.End.ChapterNumber;
    //    //        currentVerse = range.End.VerseNumber;
    //    //    }
            
    //    //    return string.Join("", rangeStrings);

    //    //}

    //    ///// <summary>
    //    ///// Determines whether this <see cref="RangeCollection_OBE"/> contains the specified <see cref="Verse"/>.
    //    ///// </summary>
    //    ///// <param name="verse">The <see cref="Verse"/>.</param>
    //    ///// <returns>
    //    /////   <c>true</c> if the <see cref="RangeCollection_OBE"/> contains the specified <see cref="Verse"/>; otherwise, <c>false</c>.
    //    ///// </returns>
    //    ///// <exception cref="ArgumentNullException">
    //    /////     <paramref name="verse"/> is null.</exception>
    //    //public bool Contains(Verse verse)
    //    //{
    //    //    if (verse == null)
    //    //        throw new ArgumentNullException("verse", "verse is null");

    //    //    return this.Any(range => range.Contains(verse));
    //    //}

    //    ///// <summary>
    //    ///// Determines whether this <see cref="RangeCollection_OBE"/> contains all of the <see cref="Verse">Verses</see>
    //    ///// in the specified <see cref="Range"/>.
    //    ///// </summary>
    //    ///// <param name="range">The <see cref="Range"/>.</param>
    //    ///// <returns>
    //    /////   <c>true</c> if this <see cref="RangeCollection_OBE"/> contains all of the <see cref="Verse">Verses</see>
    //    ///// in the specified <see cref="Range"/>; otherwise, <c>false</c>.
    //    ///// </returns>
    //    ///// <exception cref="ArgumentNullException">
    //    /////     <paramref name="range"/> is null.</exception>
    //    //new public bool Contains(Range range)
    //    //{
    //    //    if (range == null)
    //    //        throw new ArgumentNullException("range", "range is null");
            
    //    //    return this.Any(r => r.Contains(range));
    //    //}

    //    ///// <summary>
    //    ///// Determines whether this <see cref="RangeCollection_OBE"/> contains any of the <see cref="Verse">Verses</see>
    //    ///// in the specified <see cref="Range"/>.
    //    ///// </summary>
    //    ///// <param name="range">The <see cref="Range"/>.</param>
    //    ///// <returns>
    //    /////   <c>true</c> if this <see cref="RangeCollection_OBE"/> contains any of the <see cref="Verse">Verses</see>
    //    ///// in the specified <see cref="Range"/>; otherwise, <c>false</c>.
    //    ///// </returns>
    //    ///// <exception cref="ArgumentNullException">
    //    /////     <paramref name="range"/> is null.</exception>
    //    //public bool Intersects(Range range)
    //    //{
    //    //    if (range == null)
    //    //        throw new ArgumentNullException("range", "range is null");

    //    //    return this.Any(r => r.Intersects(range));
    //    //}

    //    ///// <summary>
    //    ///// Determines whether this <see cref="RangeCollection_OBE"/> contains any of the <see cref="Verse">Verses</see>
    //    ///// in the specified <see cref="RangeCollection_OBE"/>.
    //    ///// </summary>
    //    ///// <param name="range">The <see cref="RangeCollection_OBE"/>.</param>
    //    ///// <returns>
    //    /////   <c>true</c> if this <see cref="RangeCollection_OBE"/> contains any of the <see cref="Verse">Verses</see>
    //    ///// in the specified <see cref="RangeCollection_OBE"/>; otherwise, <c>false</c>.
    //    ///// </returns>
    //    ///// <exception cref="ArgumentNullException">
    //    /////     <paramref name="range"/> is null.</exception>
    //    //public bool Intersects(RangeCollection_OBE reference)
    //    //{
    //    //    throw new NotImplementedException();

    //    //    //if (reference == null)
    //    //    //    throw new ArgumentNullException("range", "range is null");

    //    //    //foreach (Range range in this)
    //    //    //    if (range.Intersects(reference))
    //    //    //        return true;
    //    //    //return false;
    //    //}

    //    //internal class EqualityComparer : IEqualityComparer<RangeCollection_OBE>
    //    //{
    //    //    public bool Equals(RangeCollection_OBE x, RangeCollection_OBE y)
    //    //    {
    //    //        return this.GetHashCode(x) == this.GetHashCode(y);
    //    //    }

    //    //    public int GetHashCode(RangeCollection_OBE obj)
    //    //    {
    //    //        int result = 0;

    //    //        foreach (Range range in obj)
    //    //        {
    //    //            result ^= range.GetHashCode();
    //    //        }
    //    //        return result;
    //    //    }
    //    //}
    //}
}
