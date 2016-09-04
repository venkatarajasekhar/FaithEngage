using System;
using NUnit.Framework;
using System.Collections.Generic;
using FakeItEasy;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.TemplatingService;

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

            public override void SetAttributes (System.Collections.Generic.Dictionary<string, string> attributes)
            {
                
            }

            public override IRenderableCard GetCard (ITemplatingService service, IDictionary<Guid, PluginFileInfo> files)
            {
                throw new NotImplementedException ();
            }

            public override DisplayUnitPlugin Plugin {
                get {
                    var plugin = A.Fake<DisplayUnitPlugin> ();
					A.CallTo (() => plugin.DisplayUnitType).Returns (typeof(dummyDisplayUnit));
                    return plugin;
                }
            }
            #endregion
                        
        }

        [TestFixtureSetUp]
        public void init()
        {
            DICT = new Dictionary<string,string> () {
                { "Name", TEST_NAME },
				{"Description", "My Description"},
                { "AssociatedEvent", TEST_GUID.ToString()},
                { "DateCreated", DateTime.Now.ToShortDateString()},
				{"GroupId", TEST_GUID.ToString()},
				{"PositionInGroup", "3"}
			};
        }


        [Test]
        public void Ctor_NoGuid_EmptyAttributes(){
			var dummy = new dummyDisplayUnit (new Dictionary<string,string>());
            Assert.That(dummy.Id, Is.InstanceOf(typeof(Guid)));
			Assert.That(dummy.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That (dummy.UnitGroup == null);
        }

        [Test]
        public void Ctor_NoGuid_AttributesInDictionary()
        {
            var dummy = new dummyDisplayUnit (DICT);
            Assert.That (dummy.Name == TEST_NAME);
            Assert.That (dummy.AssociatedEvent == TEST_GUID);
            Assert.That (dummy.DateCreated.Date == DateTime.Now.Date);
            Assert.That (dummy.Description == "My Description");
			Assert.That (dummy.UnitGroup, Is.Not.Null);
			Assert.That (dummy.UnitGroup.Value.Id, Is.EqualTo (TEST_GUID));
			Assert.That (dummy.UnitGroup.Value.Position, Is.EqualTo (3));
        }

        [Test]
        public void Ctor_Guid_AttributesInDictionary()
        {
            var dummy = new dummyDisplayUnit (TEST_GUID, DICT);
            Assert.That (dummy.Id == TEST_GUID);
			Assert.That (dummy.Name == TEST_NAME);
			Assert.That (dummy.AssociatedEvent == TEST_GUID);
			Assert.That (dummy.DateCreated.Date == DateTime.Now.Date);
			Assert.That (dummy.Description == "My Description");
			Assert.That (dummy.UnitGroup, Is.Not.Null);
			Assert.That (dummy.UnitGroup.Value.Id, Is.EqualTo (TEST_GUID));
			Assert.That (dummy.UnitGroup.Value.Position, Is.EqualTo (3));
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
			Assert.That (dummy.AssociatedEvent, Is.EqualTo (dummy2.AssociatedEvent));
			Assert.That (dummy.Description, Is.EqualTo (dummy2.Description));
			Assert.That (dummy.Plugin.PluginId, Is.EqualTo (dummy2.Plugin.PluginId));
			Assert.That (dummy.UnitGroup, Is.EqualTo (dummy2.UnitGroup));
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

