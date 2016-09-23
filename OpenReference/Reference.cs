//TODO:JF: Change OpenReference project to submodule tracking the ReferenceParser repo on my github

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Konves.Scripture
{
    public class Reference : IEnumerable<Verse>, IDisposable, IFormattable
    {
        private Scripture.Version.ScriptureInfo _scriptureInfo;

        private List<Range> _ranges;

        private Reference()
        {
            this._ranges = new List<Range>();
        }

        private static Reference Create(Version.ScriptureInfo scriptureInfo, Range range)
        {
            Reference result = new Reference();

            result._scriptureInfo = scriptureInfo;

            result.HasSubReferences = false;
            result._ranges.Add(range);
            result.Length = range.Length;
            result.FirstVerse = range.Start;
            result.LastVerse = range.End;

            return result;
        }

        private static Reference Create(Version.ScriptureInfo scriptureInfo, IEnumerable<Range> ranges)
        {
            Reference result = new Reference();

            result._scriptureInfo = scriptureInfo;

            result.HasSubReferences = ranges.Count()>1;
            result._ranges.AddRange(ranges);
            result.Length = ranges.Sum(r => r.Length);
            result.FirstVerse = ranges.First().Start;
            result.LastVerse = ranges.Last().End;

            return result;
        }

        public static bool TryParse(string s, Version.ScriptureInfo scriptureInfo, out Reference result)
        {
            if (s == null)
                throw new ArgumentNullException("s", "s is null");

            if (scriptureInfo == null)
                throw new ArgumentNullException("scriptureInfo", "scriptureInfo is null");

            //result = new Reference();

            Data.RawReference rawRef = Data.RawReference.Parse(s);

            if (rawRef.Count == 0)
            {
                result = null;
                return false;
            }
            else if (rawRef.Count == 1)
            {
                result = Create(scriptureInfo, Range.Create(scriptureInfo, rawRef[0], 0, RepositoryMode.Safe));                
            }
            else
            {
                int offset = 0;

                List<Range> ranges = new List<Range>();

                foreach (var r in rawRef)
                {
                    Range range = Range.Create(scriptureInfo, r, offset, RepositoryMode.Safe);
                    offset += range.Length;
                    ranges.Add(range);
                }

                result = Create(scriptureInfo, ranges);
            }
            return true;
        }

        /// <summary>
        /// Gets the total number of verses in this <see cref="Konves.Scripture.Reference"/>.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the <see cref="Konves.Scripture.Verse"/> at the specified index.
        /// </summary>
        public Verse this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Length)
                    throw new IndexOutOfRangeException("index is greater than or equal to the length of this object or less than zero.");

                // TODO: perfomance tune this line for large ranges
                var r = this._ranges.LastOrDefault(x => x.Offset <= index);

                return r[index - r.Offset];
            }
        }

        public bool HasSubReferences { get; private set; }

        public Verse FirstVerse { get; private set; }

        public Verse LastVerse { get; private set; }

        public IEnumerable<Reference> GetSubReferences()
        {
            if (HasSubReferences)
                return this._ranges.Select(r => Create(_scriptureInfo, r));
            else
                return null;
        }

        public IEnumerator<Verse> GetEnumerator()
        {
            return new VerseEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new VerseEnumerator(this);
        }

        void IDisposable.Dispose()
        {

        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        private class VerseEnumerator : IEnumerator<Verse>
        {
            private const int newIndex = -1;
            private int _index;
            private Reference _reference;

            /// <summary>
            /// Initializes a new instance of the <see cref="VerseEnumerator"/> class.
            /// </summary>
            /// <param name="reference">The reference.</param>
            public VerseEnumerator(Reference reference)
            {
                _index = newIndex;
                _reference = reference;
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// <c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public bool MoveNext()
            {
                _index++;
                return _index < _reference.Length;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                _index = newIndex;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            ///   </returns>
            Verse IEnumerator<Verse>.Current
            {
                get { return _reference[_index]; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                _reference = null;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            ///   </returns>
            object IEnumerator.Current
            {
                get { return _reference[_index]; }
            }
        }
        
    }

    //public class Reference : IEnumerable<Verse>, IDisposable, IFormattable
    //{
    //    private IEnumerable<ReferenceSet> _referenceSets;
    //    private Scripture.Version.ScriptureInfo _repository;
    //    private IEnumerable<VerseSummary> _verseSummaries;

    //    //private RangeCollection _rangeCollection;

    //    private List<Range> _ranges;

    //    /// <summary>
    //    /// Converts the string representation of a scripture reference to its <see cref="Reference"/> object equivalent.
    //    /// </summary>
    //    /// <param name="s">A string containing a scripture reference to convert.</param>
    //    /// <param name="scriptureInfo">The <see cref="ScriptureInfo"/> object.</param>
    //    /// <returns>
    //    /// A <see cref="Reference"/> equivalent to the scripture reference contained in <paramref name="s"/>.
    //    /// </returns>
    //    /// <exception cref="System.ArgumentNullException">
    //    ///   <paramref name="s"/> is null.</exception>
    //    ///   
    //    /// <exception cref="System.ArgumentNullException">
    //    ///   <paramref name="scriptureInfo"/> is null.</exception>
    //    ///   
    //    /// <exception cref="System.FormatException">
    //    ///   <paramref name="s"/> is not in the correct format.</exception>
    //    public static Reference Parse(string s, Version.ScriptureInfo scriptureInfo)
    //    {
    //        if (s == null)
    //            throw new ArgumentNullException("s", "s is null.");

    //        if (scriptureInfo == null)
    //            throw new ArgumentNullException("scriptureInfo", "scriptureInfo is null.");

    //        Reference reference;

    //        if (TryParse(s, scriptureInfo, out reference))
    //            return reference;
    //        else
    //            throw new FormatException("s is not in the correct format.");
    //    }

    //    public static Reference Parse(string s, string scriptureId)
    //    {
    //        return Parse(s, Version.ScriptureInfo.GetInstance(scriptureId));
    //    }

    //    /// <summary>
    //    /// Converts the string representation of a scripture reference to its <see cref="Reference"/> object equivalent.
    //    /// A return value indicates whether the conversion succeeded.
    //    /// </summary>
    //    /// <param name="s">A string containing a scripture reference to convert.</param>
    //    /// <param name="result">
    //    /// When this method returns, contains the <see cref="Reference"/> object equivalent
    //    /// to the scripture reference contained in <paramref name="s"/>, if the conversion succeeded,
    //    /// or <c>null</c> if the conversion failed. The conversion fails if the <paramref name="s"/>
    //    /// parameter is <c>null</c> or is not of the correct format. This parameter is passed uninitialized.
    //    /// </param>
    //    /// <returns>
    //    /// <c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.
    //    /// </returns>
    //    public static bool TryParse(string s, Version.ScriptureInfo scriptureInfo, out Reference result)
    //    {
    //        result = null;

    //        RangeCollection_OBE rangeCollection;




    //        if (!RangeCollection_OBE.TryParse(s, scriptureInfo, out rangeCollection))
    //            return false;

    //        result = new Reference();
    //        result._repository = scriptureInfo;
    //        //result._rangeCollection = rangeCollection;
    //        result._ranges = new List<Range>();

    //        Data.RawReference rawRef = Data.RawReference.Parse(s);


    //        foreach (Data.RawRange range in rawRef)
    //        {
    //            var x = Range.Create(scriptureInfo, range, RepositoryMode.Safe /* figure out what to do with mode */);

    //            if (x == null)
    //            {
    //                //if (mode == RepositoryMode.Strict)
    //                //{
    //                //    reference = null;
    //                //    return false;
    //                //}
    //            }
    //            else
    //                result._ranges.Add(x);
    //        }

    //        if (!result._ranges.Any())
    //            return false;

    //        if (result._ranges.Length == 1)
    //        {
    //            Range range = result._ranges.First();

    //            result.init(scriptureInfo, range);
    //        }
    //        else
    //        {
    //            var references = (
    //                from range in result._ranges
    //                select new Reference(scriptureInfo, range)
    //                ).ToArray();

    //            // BUGS BE HERE (but i need to go to bed ...)
    //            result._referenceSets = GetReferenceSets(references);

    //            result.Length = references.Sum(vc => vc.Length);
    //        }

    //        return true;
    //    }

    //    public static bool TryParse(string s, string scriptureId, out Reference result)
    //    {
    //        return TryParse(s, Version.ScriptureInfo.GetInstance(scriptureId), out result);
    //    }

    //    #region Costructors
    //    /// <summary>
    //    /// Prevents a default instances of the <see cref="Reference"/> class from being created.
    //    /// </summary>
    //    /// <param name="range">The range.</param>
    //    private Reference(Version.ScriptureInfo scriptureInfo, Range range)
    //    {
    //        init(scriptureInfo, range);
    //    }

    //    private Reference() { }

    //    private void init(Version.ScriptureInfo scriptureInfo, Range range)
    //    {
    //        _repository = scriptureInfo;
    //        //_rangeCollection = new RangeCollection() { range };
    //        _ranges = new List<Range> { range };

    //        var start = range.Start;
    //        var end = range.End;

    //        var limits = scriptureInfo.GetChapterLimits(start.BookNumber, start.ChapterNumber, end.BookNumber, end.ChapterNumber);

    //        _verseSummaries = GetVerseSummaries(limits, start.VerseNumber);
    //        this.Length = _verseSummaries.Last().Offset + range.End.VerseNumber;

    //        string[] asdf = new string[] { };
    //    }
    //    #endregion
        
    //    private static IEnumerable<ReferenceSet> GetReferenceSets(Reference[] references)
    //    {
    //        for (int i = 0; i < references.Length; i++)
    //        {
    //            yield return new ReferenceSet
    //            {
    //                Reference = references[i],
    //                StartIndex = references.Take(i).Sum(vc => vc.Length),
    //                EndIndex = references.Take(i).Sum(vc => vc.Length) + references[i].Length - 1
    //            };
    //        }
    //    }
        
    //    private static IEnumerable<VerseSummary> GetVerseSummaries(IEnumerable<Version.ScriptureInfo.ChapterLimits> chapterLimits, int startVerse)
    //    {
    //        return
    //            from limit in chapterLimits
    //            select new VerseSummary
    //            {
    //                Book = limit.BookNumber,
    //                Chapter = limit.ChapterNumber,
    //                Verses = limit.EndVerseNumber,
    //                Offset = (
    //                    from l in chapterLimits
    //                    where
    //                        l.BookNumber < limit.BookNumber
    //                        || (l.BookNumber == limit.BookNumber && l.ChapterNumber < limit.ChapterNumber)

    //                    select l.EndVerseNumber).Sum() - startVerse + 1
    //            };
    //    }

    //    public string TranslationAbbreviation { get { return _repository.TranslationAbbreviation; } }
    //    public string Translation { get { return _repository.TranslationName; } }
    //    public string ScriptureName { get { return _repository.Name; } }

    //    /// <summary>
    //    /// Returns a <see cref="System.String"/> that represents this instances.
    //    /// </summary>
    //    /// <returns>
    //    /// A <see cref="System.String"/> that represents this instances.
    //    /// </returns>
    //    public override string ToString()
    //    {
    //        return this.ToString(string.Empty, null);
    //    }

    //    /// <summary>
    //    /// Gets a 32-bit integer that represents the total number of
    //    /// verses in the <see cref="Konves.Scripture.Reference"/>.
    //    /// </summary>
    //    public int Length { get; private set; }

    //    /// <summary>
    //    /// Gets the <see cref="Konves.Scripture.Verse"/> at the specified index.
    //    /// </summary>
    //    public Verse this[int index]
    //    {
    //        get
    //        {
    //            if (_verseSummaries != null) // single collection
    //            {
    //                var summary = (
    //                    from vs in _verseSummaries
    //                    where vs.Offset <= index
    //                    select vs
    //                    ).LastOrDefault();

    //                int book = summary.Book;
    //                int chapter = summary.Chapter;
    //                int verse = index - summary.Offset + 1;

    //                string bookName = _repository.GetBookInfo(book).Name;

    //                Verse v = Verse.Create(_repository, bookName, chapter, verse, null, VersePosition.First, RepositoryMode.Safe);

    //                if (v == null)
    //                {
    //                    int asdf = 234;
    //                }

    //                return v;
    //            }
    //            else // multi-collection
    //            {
    //                var verseCollectionSet = _referenceSets.Single(vc => index.Between(vc.StartIndex, vc.EndIndex));

    //                return verseCollectionSet.Reference[index - verseCollectionSet.StartIndex];
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the first verse.
    //    /// </summary>
    //    public Verse FirstVerse
    //    {
    //        get
    //        {
    //            //return this._rangeCollection.First().Start;
    //            return this._ranges.First().Start;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the last verse.
    //    /// </summary>
    //    public Verse LastVerse
    //    {
    //        get
    //        {
    //            //return this._rangeCollection.Last().End;
    //            return this._ranges.Last().End;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets a value indicating whether this instances has sub-references.
    //    /// </summary>
    //    /// <value>
    //    /// 	<c>true</c> if this instances has sub-references; otherwise, <c>false</c>.
    //    /// </value>
    //    public bool HasSubReferences
    //    {
    //        get
    //        {
    //            return _verseSummaries == null;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the sub-references.
    //    /// </summary>
    //    /// <returns></returns>
    //    public IEnumerable<Reference> GetSubReferences()
    //    {
    //        if (_referenceSets == null)
    //            return new Reference[] { this };
    //        else
    //            return _referenceSets.Select(vcs => vcs.Reference);
    //    }
        
    //    #region IEnumerable<Verse>
    //    private class VerseEnumerator : IEnumerator<Verse>
    //    {
    //        private const int newIndex = -1;

    //        public VerseEnumerator(Reference reference)
    //        {
    //            _index = newIndex;
    //            _reference = reference;
    //        }

    //        private int _index;
    //        private Reference _reference;

    //        public bool MoveNext()
    //        {
    //            _index++;
    //            return _index < _reference.Length;
    //        }

    //        public void Reset()
    //        {
    //            _index = newIndex;
    //        }

    //        Verse IEnumerator<Verse>.Current
    //        {
    //            get { return _reference[_index]; }
    //        }

    //        public void Dispose()
    //        {
    //            _reference = null;
    //        }

    //        object IEnumerator.Current
    //        {
    //            get { return _reference[_index]; }
    //        }
    //    }

    //    public IEnumerator<Verse> GetEnumerator()
    //    {
    //        var x = new VerseEnumerator(this);

    //        return x;
    //    } 
    //    #endregion

    //    #region IEnumerable
    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return new VerseEnumerator(this);
    //    } 
    //    #endregion

    //    #region IDisposable
    //    public void Dispose()
    //    {
    //        _repository = null;
    //        _verseSummaries = null;
    //        _referenceSets = null;
    //    } 
    //    #endregion

    //    internal class VerseSummary
    //    {
    //        internal int Book { get; set; }
    //        internal int Chapter { get; set; }
    //        internal int Offset { get; set; }
    //        internal int Verses { get; set; }
    //    }

    //    private class ReferenceSet
    //    {
    //        internal Reference Reference { get; set; }
    //        internal int StartIndex { get; set; }
    //        internal int EndIndex { get; set; }
    //    }

    //    /// <summary>
    //    /// Returns a <see cref="System.String"/> that represents this instance.
    //    /// </summary>
    //    /// <param name="format">The format.</param>
    //    /// <param name="formatProvider">The format provider.</param>
    //    /// <returns>
    //    /// A <see cref="System.String"/> that represents this instance.
    //    /// </returns>
    //    public string ToString(string format, IFormatProvider formatProvider)
    //    {
    //        IFormatProvider provider = formatProvider ?? DefaultFormatProvider.Instance;

    //        object obj = provider.GetFormat(this.GetType());

    //        if (obj is ICustomFormatter)
    //        {
    //            return (obj as ICustomFormatter).Format(format, this, provider);
    //        }
    //        else
    //            return null;
    //    }
    //}    
}
