using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using FakeItEasy;
using NUnit.Framework;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.DisplayUnits.Factories;

namespace FaithEngage.Core.DisplayUnits
{
    [TestFixture]
    public class DisplayUnitFactoryTests
    {
        public const string VALID_STRING = "VALID STRING";
        public const string INVALID_STRING = "INVALID STRING";
		private readonly IDisplayUnitPluginContainer _container = A.Fake<IDisplayUnitPluginContainer>();
        private readonly DisplayUnitPlugin _plgin = A.Fake<DisplayUnitPlugin> ();
        private DisplayUnitDTO _validDtoNoId;
        private DisplayUnitDTO _validDtoWithId;
        private DisplayUnitDTO _invalidDto;
        private Guid VALID_GUID;
        private Guid INVALID_GUID;

		private class DummyDisplayUnit : DisplayUnit
		{
			public DummyDisplayUnit (Dictionary<string, string> attributes) : base (attributes)
			{
			}
			public DummyDisplayUnit (Guid id, Dictionary<string, string> attributes) : base (id, attributes)
			{
			}
			public override Dictionary<string, string> GetAttributes ()
			{
				throw new NotImplementedException ();
			}

			public override FaithEngage.Core.Cards.Interfaces.IRenderableCard GetCard ()
			{
				throw new NotImplementedException ();
			}

			public override void SetAttributes (Dictionary<string, string> attributes)
			{
				throw new NotImplementedException ();
			}

			public override DisplayUnitPlugin Plugin {
				get {
					return A.Fake<DisplayUnitPlugin> ();
				}
			}
		}
			
		[TestFixtureSetUp]
        public void init()
        {
            VALID_GUID = Guid.NewGuid ();
            INVALID_GUID = Guid.NewGuid ();
            _validDtoWithId = new DisplayUnitDTO (VALID_GUID, VALID_GUID) {
                DateCreated = DateTime.Now.Date,
                Description = "My Description",
                PositionInEvent = 2,
                GroupId = VALID_GUID,
                PositionInGroup = 5,
                PluginId = VALID_GUID
            };

            _validDtoNoId = new DisplayUnitDTO (VALID_GUID) {
                DateCreated = DateTime.Now.Date,
                Description = "My Description",
                PositionInEvent = 2,
                GroupId = VALID_GUID,
                PluginId = VALID_GUID
            };

            _invalidDto = new DisplayUnitDTO (VALID_GUID) {
                PluginId = INVALID_GUID,
                PositionInEvent = 2,
            };


            A.CallTo (() => _container.Resolve (VALID_GUID)).Returns (_plgin);
            A.CallTo (() => _container.Resolve (INVALID_GUID)).Returns (null);
			A.CallTo (() => _plgin.DisplayUnitType).Returns (typeof(DummyDisplayUnit));
        }

        [Test]
        public void InstantiateNew_ValidPluginID_EmptyDict_ReturnsCorrectDisplayUnit()
        {
            var factory = new DisplayUnitFactory (_container);

            var du = factory.InstantiateNew (VALID_GUID, new Dictionary<string, string> ());

			Assert.That (du, Is.InstanceOf(typeof(DummyDisplayUnit)));
        }

        [Test]
        public void InstantiateNew_ValidPluginID_LoadedDict_ReturnsCorrectDisplayUnit()
        {
            var dict = new Dictionary<string,string> () {
                { "PositionInEvent", "1" },
                { "DateCreated", DateTime.Now.ToShortDateString() }
            };
            var factory = new DisplayUnitFactory (_container);
            var du = factory.InstantiateNew (VALID_GUID, dict);

			Assert.That (du, Is.InstanceOf (typeof(DummyDisplayUnit)));
            Assert.That (du.PositionInEvent == 1);
            Assert.That (du.DateCreated == DateTime.Now.Date);
        }

        [Test]
        [ExpectedException(typeof(NotRegisteredPluginException))]
        public void InstantiateNew_InvalidPluginId_ThrowsException()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.InstantiateNew (INVALID_GUID, new Dictionary<string, string> ());
        }

        [Test]
        public void ConvertFromDto_ValidDtoWithoutId_NewDisplayUnit()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.ConvertFromDto (_validDtoNoId);

            Assert.That (du.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That (du.PositionInEvent, Is.EqualTo(2));
            Assert.That (du, Is.InstanceOf (typeof(DisplayUnit)));
            Assert.That (du.AssociatedEvent, Is.EqualTo (VALID_GUID));
            Assert.That (du.DateCreated, Is.EqualTo (DateTime.Now.Date));
        }

        [Test]
        public void ConvertFromDto_ValidDtoWithId_NewDisplayUnit()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.ConvertFromDto (_validDtoWithId);

            Assert.That (du.Id, Is.EqualTo(VALID_GUID));
            Assert.That (du.PositionInEvent, Is.EqualTo(2));
            Assert.That (du, Is.InstanceOf (typeof(DisplayUnit)));
            Assert.That (du.AssociatedEvent, Is.EqualTo (VALID_GUID));
            Assert.That (du.DateCreated, Is.EqualTo (DateTime.Now.Date));
        }

        [Test]
        public void ConvertFromDto_InvalidDTOPlugin_ReturnsNull()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.ConvertFromDto (_invalidDto);
            Assert.That (du, Is.Null);
        }



    }
}

