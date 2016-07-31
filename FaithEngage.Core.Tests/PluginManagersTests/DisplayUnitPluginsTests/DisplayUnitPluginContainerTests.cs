using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.DisplayUnits;
using System.Collections.Generic;
using FaithEngage.Core.Tests;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	[TestFixture]
    public class DisplayUnitPluginContainerTests
    {
        private DisplayUnitPlugin _plgn;
		private Guid VALID_GUID = Guid.NewGuid();
		private Guid INVALID_GUID = Guid.NewGuid();

        private class dummy_NotDisplayUnit
        {
            public dummy_NotDisplayUnit (System.Collections.Generic.Dictionary<string, string> attributes)
            {
            }
            

            public dummy_NotDisplayUnit (Guid id, System.Collections.Generic.Dictionary<string, string> attributes)
            {
            }
            
        }

        private class dummy_DisplayUnit : DisplayUnit
        {
            public dummy_DisplayUnit (Dictionary<string, string> attributes) : base (attributes)
            {
            }

            public dummy_DisplayUnit (Guid id, Dictionary<string, string> attributes) : base (id, attributes)
            {
            }


            #region implemented abstract members of DisplayUnit
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
            #endregion
        }

		[SetUp]
        public void init()
        {
            _plgn = A.Fake<DisplayUnitPlugin> ();
        }

        [Test]
        public void Register_ValidPlugin_NoExceptions()
        {
			_plgn.PluginId = VALID_GUID;
			A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_DisplayUnit));
            var pcontainer = new DisplayUnitPluginContainer ();
            pcontainer.Register (_plgn);
        }

		[Test]
		[ExpectedException(typeof(PluginHasInvalidIdException))]
		public void Register_NullPluginId_ThrowsPluginHasInvalidIdException()
		{
			A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_DisplayUnit));
			var pcontainer = new DisplayUnitPluginContainer ();
			pcontainer.Register (_plgn);
		}

		[Test]
		[ExpectedException(typeof(PluginHasInvalidIdException))]
		public void Register_PluginIdIEmptyGuid_ThrowsPluginHasInvalidIdException()
		{
			_plgn.PluginId = Guid.Empty;
			A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_DisplayUnit));
			var pcontainer = new DisplayUnitPluginContainer ();
			pcontainer.Register (_plgn);
		}

        [Test]
        [ExpectedException(typeof(PluginHasInvalidConstructorsException))]
        public void Register_PluginWithInvalidConstructors_ThrowsException()
        {
			_plgn.PluginId = VALID_GUID;
			A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(Dummy_NoParameters));
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);
        }

        [Test]
        [ExpectedException(typeof(NotDisplayUnitException))]
        public void Register_PluginWithInvalidDisplayUnitType_ThrowsException()
        {
			_plgn.PluginId = VALID_GUID;
			A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_NotDisplayUnit));
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);
        }
            
        [Test]
        [ExpectedException(typeof(PluginAlreadyRegisteredException))]
        public void Register_SamePluginIDTwice_ThrowsException()
		{
			_plgn.PluginId = VALID_GUID;
			A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_DisplayUnit));
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);
            pc.Register (_plgn);
        }

        [Test]
        public void Resolve_ValidID_ReturnsValidPlugin()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_DisplayUnit));
            A.CallTo (() => _plgn.PluginName).Returns ("Test");
			_plgn.PluginId = VALID_GUID;
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);


			var plugin = pc.Resolve (_plgn.PluginId.Value);

            Assert.That (plugin, Is.Not.Null);
            Assert.That (plugin, Is.EqualTo (_plgn));
            Assert.That (plugin.PluginName == "Test");
            Assert.That (plugin.DisplayUnitType == typeof(dummy_DisplayUnit));
        }

        [Test]
        public void Resolve_InvalidId_ReturnsNull()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_DisplayUnit));
            var pc = new DisplayUnitPluginContainer ();
			var plugin = pc.Resolve (INVALID_GUID);

            Assert.That (plugin, Is.Null);
        }
            
    }
}

