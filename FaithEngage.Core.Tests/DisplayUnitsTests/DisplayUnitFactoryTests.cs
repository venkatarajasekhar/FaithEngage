using System;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FakeItEasy;
using NUnit.Framework;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnits;
using System.Reflection;

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
        private Guid _guid;

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
					throw new NotImplementedException ();
				}
			}
		}
			
		[TestFixtureSetUp]
        public void init()
        {
            _guid = Guid.NewGuid ();

            _validDtoWithId = new DisplayUnitDTO (_guid, _guid);
            _validDtoWithId.AssociatedEvent = _guid;
            _validDtoWithId.PluginId = VALID_STRING;
            _validDtoWithId.PositionInEvent = 1;
            _validDtoWithId.Attributes = new Dictionary<string,string> (){{"Text", VALID_STRING}};

            _validDtoNoId = new DisplayUnitDTO (_guid, _guid);
            _validDtoNoId.PluginId = VALID_STRING;
            _validDtoNoId.PositionInEvent = 1;
            _validDtoNoId.Attributes = new Dictionary<string,string> (){{"Text", VALID_STRING}};

            _invalidDto = new DisplayUnitDTO (_guid, _guid);
            _invalidDto.PluginId = INVALID_STRING;
            _invalidDto.PositionInEvent = 1;
            _invalidDto.Attributes = new Dictionary<string,string> ();


            A.CallTo (() => _container.Resolve (VALID_STRING)).Returns (_plgin);
            A.CallTo (() => _container.Resolve (INVALID_STRING)).Returns (null);
			A.CallTo (() => _plgin.DisplayUnitType).Returns (typeof(DummyDisplayUnit));
        }

        [Test]
        public void InstantiateNew_ValidPluginID_EmptyDict_ReturnsCorrectDisplayUnit()
        {
            var factory = new DisplayUnitFactory (_container);

            var du = factory.InstantiateNew (VALID_STRING, new Dictionary<string, string> ());

			Assert.That (du, Is.InstanceOf(typeof(DummyDisplayUnit)));
        }

        [Test]
        public void InstantiateNew_ValidPluginID_LoadedDict_ReturnsCorrectDisplayUnit()
        {
            var dict = new Dictionary<string,string> () {
                { "Text", VALID_STRING },
                { "PositionInEvent", "1" },
                { "DateCreated", DateTime.Now.ToShortDateString() }
            };
            var factory = new DisplayUnitFactory (_container);
            var du = factory.InstantiateNew (VALID_STRING, dict);

            //Assert.That (du, Is.InstanceOf (typeof(TextUnit)));
            Assert.That (du.PositionInEvent == 1);
            Assert.That (du.DateCreated == DateTime.Now.Date);
        }

        [Test]
        [ExpectedException(typeof(NotRegisteredPluginException))]
        public void InstantiateNew_InvalidPluginId_ThrowsException()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.InstantiateNew (INVALID_STRING, new Dictionary<string, string> ());
        }

        [Test]
        public void ConvertFromDto_ValidDtoWithoutId_NewDisplayUnit()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.ConvertFromDto (_validDtoNoId);

            //Assert.That (du, Is.InstanceOf (typeof(TextUnit)));
            Assert.That (du.Id != Guid.Empty);
            Assert.That (du.PositionInEvent == 1);
            Assert.That (du.GetAttribute ("Text") == VALID_STRING);
        }

        [Test]
        public void ConvertFromDto_ValidDtoWithId_NewDisplayUnit()
        {
            var factory = new DisplayUnitFactory (_container);
            var du = factory.ConvertFromDto (_validDtoWithId);

            //Assert.That (du, Is.InstanceOf (typeof(TextUnit)));
            Assert.That (du.Id == _guid);
            Assert.That (du.PositionInEvent == 1);
            Assert.That (du.GetAttribute ("Text") == VALID_STRING);
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

