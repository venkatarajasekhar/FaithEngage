using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.PluginContainers;

namespace FaithEngage.Core.Tests
{
    [TestFixture]
    public class DisplayUnitPluginContainerTests
    {
        private DisplayUnitPlugin _plgn;

        private class dummy_NotDisplayUnit
        {
            public dummy_NotDisplayUnit (System.Collections.Generic.Dictionary<string, string> attributes)
            {
            }
            

            public dummy_NotDisplayUnit (Guid id, System.Collections.Generic.Dictionary<string, string> attributes)
            {
            }
            
        }

        [TestFixtureSetUp]
        public void init()
        {
            _plgn = A.Fake<DisplayUnitPlugin> ();
        }

        [Test]
        public void Register_ValidPlugin_NoExceptions()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(TextUnit));
            var pcontainer = new DisplayUnitPluginContainer ();
            pcontainer.Register (_plgn);
        }

        [Test]
        [ExpectedException(typeof(PluginHasInvalidConstructorsException))]
        public void Register_PluginWithInvalidConstructors_ThrowsException()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(Dummy_NoParameters));
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);
        }

        [Test]
        [ExpectedException(typeof(NotDisplayUnitException))]
        public void Register_PluginWithInvalidDisplayUnitType_ThrowsException()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(dummy_NotDisplayUnit));
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);
        }
            
        [Test]
        [ExpectedException(typeof(PluginAlreadyRegisteredException))]
        public void Register_SamePluginIDTwice_ThrowsException(){
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(TextUnit));
            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);
            pc.Register (_plgn);
        }

        [Test]
        public void Resolve_ValidID_ReturnsValidPlugin()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(TextUnit));
            A.CallTo (() => _plgn.PluginName).Returns ("Test");

            var pc = new DisplayUnitPluginContainer ();
            pc.Register (_plgn);


            var plugin = pc.Resolve (_plgn.PluginId);

            Assert.That (plugin, Is.Not.Null);
            Assert.That (plugin, Is.EqualTo (_plgn));
            Assert.That (plugin.PluginName == "Test");
            Assert.That (plugin.DisplayUnitType == typeof(TextUnit));
        }

        [Test]
        public void Resolve_InvalidId_ReturnsNull()
        {
            A.CallTo (() => _plgn.DisplayUnitType).Returns (typeof(TextUnit));

            var pc = new DisplayUnitPluginContainer ();
            var plugin = pc.Resolve (_plgn.PluginId);

            Assert.That (plugin, Is.Null);
        }
            
    }
}

