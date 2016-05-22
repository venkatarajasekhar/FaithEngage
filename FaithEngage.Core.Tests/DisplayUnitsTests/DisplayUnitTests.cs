using System;
using NUnit.Framework;
using System.Collections.Generic;
using FakeItEasy;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;


namespace FaithEngage.Core.DisplayUnits
{
    [TestFixture]
    public class DisplayUnitTests
    {
        const string TEST_NAME = "TESTNAME";
        private Guid TEST_GUID = Guid.NewGuid();
        public Dictionary<string,string> DICT;

        private class dummyDisplayUnit : DisplayUnit
        {

            public dummyDisplayUnit (System.Collections.Generic.Dictionary<string, string> attributes) : base (attributes)
            {
            }
            public dummyDisplayUnit (Guid id, System.Collections.Generic.Dictionary<string, string> attributes) : base (id, attributes)
            {
            }
            
            #region implemented abstract members of DisplayUnit
            public override Dictionary<string, string> GetAttributes ()
            {
                return new Dictionary<string, string> () {{"key", "value"}};
            }
            public override FaithEngage.Core.Cards.Interfaces.IRenderableCard GetCard ()
            {
                throw new NotImplementedException ();
            }
            public override void SetAttributes (System.Collections.Generic.Dictionary<string, string> attributes)
            {
                throw new NotImplementedException ();
            }
            public override DisplayUnitPlugin Plugin {
                get {
                    var plugin = A.Fake<DisplayUnitPlugin> ();
                    A.CallTo (() => plugin.DisplayUnitType).Returns (typeof(dummyDisplayUnit));
                    return plugin;
                }
                set {
                    throw new NotImplementedException ();
                }
            }
            #endregion
                        
        }

        [TestFixtureSetUp]
        public void init()
        {
            DICT = new Dictionary<string,string> () {
                { "Name", TEST_NAME },
                { "AssociatedEvent", TEST_GUID.ToString()},
                { "DateCreated", DateTime.Now.ToShortDateString()}
            };
        }


        [Test]
        public void Ctor_NoGuid_EmptyAttributes(){
            var dummy = new dummyDisplayUnit (new Dictionary<string,string>());
            Assert.That(dummy.Id, Is.InstanceOf(typeof(Guid)));
            Assert.That(dummy.Id.ToString(), Is.Not.StringContaining("00000000-0000-0000-0000-000000000000"));
            Assert.That (dummy.UnitGroup == null);
        }

        [Test]
        public void Ctor_NoGuid_AttributesInDictionary()
        {
            var dummy = new dummyDisplayUnit (DICT);
            Assert.That (dummy.Name == TEST_NAME);
            Assert.That (dummy.AssociatedEvent == TEST_GUID);
            Assert.That (dummy.DateCreated.Date == DateTime.Now.Date);
            Assert.That (dummy.Description == null);
        }

        [Test]
        public void Ctor_Guid_AttributesInDictionary()
        {
            var dummy = new dummyDisplayUnit (TEST_GUID, DICT);
            Assert.That (dummy.Id == TEST_GUID);
        }

        [Test]
        [ExpectedException(typeof(EmptyGuidException))]
        public void Ctor_EmptyGuid_ThrowsEmptyGuidException()
        {
            var dummy = new dummyDisplayUnit (Guid.Empty, DICT);
        }

        [Test]
        [ExpectedException(typeof(NegativePositionException))]
        public void Set_PositionInEvent_ThrowsException()
        {
            var dummy = new dummyDisplayUnit (DICT);
            dummy.PositionInEvent = -1;
        }


        [Test]
        public void Clone_Returns_Clone()
        {
            var dummy = new dummyDisplayUnit (TEST_GUID, DICT);
            var dummy2 = dummy.Clone ();
            Assert.That (dummy2, Is.InstanceOf (typeof(dummyDisplayUnit)));
            Assert.That (dummy2.Id != dummy.Id);
            Assert.That (dummy2.PositionInEvent == dummy.PositionInEvent + 1);
        }

        [Test]
        public void GetAttribute_ValidKey_ReturnsValue()
        {
            var dummy = new dummyDisplayUnit (TEST_GUID, DICT);
            var val = dummy.GetAttribute ("key");
            Assert.That (val == "value");
        }

        [Test]
        public void GetAttribute_InvalidKey_ReturnsNull()
        {
            var dummy = new dummyDisplayUnit (TEST_GUID, DICT);
            var val = dummy.GetAttribute ("blue");
            Assert.That (val, Is.Null);
        }

    }
}

