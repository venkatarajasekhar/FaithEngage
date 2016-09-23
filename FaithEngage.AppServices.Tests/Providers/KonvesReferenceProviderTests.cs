using NUnit.Framework;
using System;
using FaithEngage.AppServices;
using FaithEngage.AppServices.Providers;
using System.Security.Cryptography;
using FakeItEasy;
using FaithEngage.CorePlugins.Interfaces;

namespace FaithEngage.AppServices.Tests.Providers
{
    [TestFixture ()]
    public class KonvesReferenceProviderTests
    {
        const string VALID_SINGLE_REFERENCE = "1 John 1:9";
        const string VALID_MULTIPLE_REFERENCE = "1 John 1:9-10, Mt 5:1-10";
        const string INVALID_REFERENCE = "1 Galatians 12:9";

        [Test ()]
        public void Ctor_ValidKonvesReferenceProvider()
        {
            var krp = new KonvesReferenceProvider ();
            Assert.That(krp, Is.InstanceOf(typeof(KonvesReferenceProvider)));
        }

        [Test]
        public void Parse_ValidSingleReference_ValidIReference ()
        {
            var krp = new KonvesReferenceProvider ();
            var reference = krp.Parse (VALID_SINGLE_REFERENCE);

            Assert.That (reference, Is.InstanceOf (typeof(IReference)));
            Assert.That (reference.HasSubReferences == false);
        }

        [Test]
        public void Parse_ValidMultipleReference_ValidIReference ()
        {
            var krp = new KonvesReferenceProvider ();
            var reference = krp.Parse (VALID_MULTIPLE_REFERENCE);

            Assert.That (reference, Is.InstanceOf (typeof(IReference)));
            Assert.That (reference.HasSubReferences == true);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.ScriptureReferenceParseException")]
        public void Parse_InvalidReference_ThrowsScriptureReferenceParseException()
        {
            var krp = new KonvesReferenceProvider ();
            var reference = krp.Parse (INVALID_REFERENCE);

            Assert.That (reference, Is.Null);
        }

        [Test]
        public void GetReference_ValidSingleReference_ReturnsReference(){
            var krp = new KonvesReferenceProvider();
            var reference = krp.Parse(VALID_SINGLE_REFERENCE);

            var refString = krp.GetReference (reference);

            Assert.That (refString.Length > 0);
            Assert.That (refString, Contains.Substring ("1 John 1:9"));
        }

        [Test]
        public void GetReference_ValidMultipleReference_ReturnsReference(){
            var krp = new KonvesReferenceProvider();
            var reference = krp.Parse(VALID_MULTIPLE_REFERENCE);

            var refString = krp.GetReference (reference);

            Assert.That (refString.Length > 0);
            Assert.That (refString, Contains.Substring ("Matthew 5:1"));
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.UnParsedReferenceObjectException")]
        public void GetReference_InvalidReference_ThrowsUnParsedReferenceObjectException()
        {
            var krp = new KonvesReferenceProvider ();
            var reference = A.Fake<IReference> ();
            A.CallTo (() => reference.IsParsed).Returns (false);

            var refString = krp.GetReference (reference);
        }
    }
}

