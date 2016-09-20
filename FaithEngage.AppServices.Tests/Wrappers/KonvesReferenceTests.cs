using System;
using NUnit.Framework;
using FakeItEasy;
using Konves.Scripture;
using FaithEngage.AppServices.Wrappers;
using FaithEngage.Core.Interfaces;
using FaithEngage.AppServices.Providers;
using Konves.Scripture.Version;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy.Configuration;

namespace FaithEngage.AppServices.Tests.Wrappers
{
    [TestFixture]
    public class KonvesReferenceTests
    {
        const string VALID_SINGLE_REFERENCE = "1 John 1:9";
        const string VALID_MULTIPLE_REFERENCE = "1 John 1:9-10, Mt 5:1-10";
        const string INVALID_REFERENCE = "1 Galatians 12:9";

        private ScriptureInfo _si;
        private Reference _validSingleReference;
        private Reference _validMultipleReference;



        [TestFixtureSetUp]
        public void Init()
        {
            ScriptureInfo.TryRegister ("esv", "Data/esv.xml");
            this._si = ScriptureInfo.GetInstance ("esv");


            Reference.TryParse (VALID_SINGLE_REFERENCE, _si, out _validSingleReference);
            Reference.TryParse (VALID_MULTIPLE_REFERENCE, _si, out _validMultipleReference);  
        }

        [Test]
        public void Ctor_ValidSingleReference_ValidKonvesReference()
        {

            var kref = new KonvesReference (_validSingleReference);

            Assert.That(kref, Is.InstanceOf(typeof(KonvesReference)));
            Assert.That (kref.IsParsed == true);
            Assert.That (kref, Is.InstanceOf (typeof(IReference)));
            Assert.That (kref.Length > 0);
        }

        [Test]
        public void Ctor_ValidMultipleReference_ValidKonvesReference()
        {

            var kref = new KonvesReference (_validMultipleReference);

            Assert.That(kref, Is.InstanceOf(typeof(KonvesReference)));
            Assert.That (kref.IsParsed == true);
            Assert.That (kref, Is.InstanceOf (typeof(IReference)));
            Assert.That (kref.Length > 0);
        }

        [Test]
        public void GetSubReferences_ValidSingleReference_ReturnsNull()
        {
            var kref = new KonvesReference (_validSingleReference);
            var subRefs = kref.GetSubReferences ();
            Assert.That (subRefs, Is.Null);
        }

        [Test]
        public void GetSubReferences_ValidMultiplereference_ReturnsIEnumerable_Of_IReference()
        {
            var kref = new KonvesReference (_validMultipleReference);
            var subRefs = kref.GetSubReferences ();

            Assert.That (subRefs, Is.InstanceOf (typeof(IEnumerable<IReference>)));
            Assert.That (subRefs.Count () == 2);
            Assert.That (kref.Length > 0);
        } 

    }
}

