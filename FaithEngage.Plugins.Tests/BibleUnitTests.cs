using System;
using NUnit.Framework;
using FaithEngage.Core.DisplayUnits;
using NUnit.Framework.Constraints;
using FakeItEasy;
using FaithEngage.Core.Interfaces;
using FaithEngage.Core.Exceptions;
using System.ComponentModel;

namespace FaithEngage.Core.Tests
{
    [TestFixture()]
    public class BibleUnitTests
    {
        const string VALID_SINGLE_REFERENCE = "1 John 1:9";
        const string VALID_MULTIPLE_REFERENCE = "1 John 1:9-10, Mt 5:1-10";
        const string INVALID_REFERENCE = "1 Galatians 12:9";

        private IReferenceProvider _refProvider;

        [TestFixtureSetUp]
        public void Init(){
            _refProvider = A.Fake < IReferenceProvider> ();
        }


        [Test()]
        public void Constructor_ValidSingleReference_BibleUnit(){
            var bu = new BibleUnit (VALID_SINGLE_REFERENCE, _refProvider);

            Assert.That(bu,Is.InstanceOf(typeof(BibleUnit)));
        }

        [Test()]
        public void Constructor_ValidMultipelReference_BibleUnit(){
            var bu = new BibleUnit (VALID_MULTIPLE_REFERENCE, _refProvider);
            Assert.That (bu, Is.InstanceOf (typeof(BibleUnit)));
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.ScriptureReferenceParseException")]
        public void Constructor_InvalidReference_ScriptureReferenceParseException(){
            A.CallTo (() => _refProvider.Parse (INVALID_REFERENCE)).Throws (new ScriptureReferenceParseException (INVALID_REFERENCE));

            var bu = new BibleUnit (INVALID_REFERENCE, _refProvider);
        }

        [Test]
        public void GetReference_ValidReference_ReturnsReference(){
            A.CallTo (() => _refProvider.GetReference (A<IReference>.Ignored)).Returns ("1 John 1:9-1 John 1010, Matthew 5:1 - Matthew 5:10");

            var bu = new BibleUnit (VALID_MULTIPLE_REFERENCE, _refProvider);
            var reference = bu.GetReference ();

            Assert.That (reference.Length > 0);
            Assert.That (reference, Contains.Substring ("Matthew"));
        }
    }
}

