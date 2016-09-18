using NUnit.Framework;
using FaithEngage.Core.Factories;
using FakeItEasy;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using System.IO;
using System.Linq;
using FaithEngage.Core.PluginManagers.Files;
using System;

namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
{
    [TestFixture]
    public class TextUnitPluginTests
    {
        private IAppFactory _appFac;

        [SetUp]
        public void Init()
        {
            _appFac = A.Fake<IAppFactory> ();
        }

        [Test]
        public void Ctor_InitializesAllProperties()
        {
            var tup = new TextUnitPlugin ();

            Assert.That (tup.GetAttributeNames ().Count == 1);
            Assert.That (tup.GetAttributeNames () [0] == "Text");
            Assert.That (tup.DisplayUnitType == typeof (TextUnit));
            Assert.That (tup.EditorDefinition, Is.InstanceOf<TextUnitEditorDefinition> ());
        }

        [Test]
        public void Initialize_RegistersTemplates()
        {
            Assert.Inconclusive ("Needs Review due to massive changes.");
            var tup = new TextUnitPlugin ();

            var tempService = A.Fake<ITemplatingService> ();
            var fileMgr = A.Fake<IPluginFileManager> ();

            A.CallTo (() => _appFac.TemplatingService).Returns (tempService);
            A.CallTo (() => _appFac.PluginFileManager).Returns (fileMgr);

            var plugDir = new DirectoryInfo (Path.Combine (AppDomain.CurrentDomain.BaseDirectory,"Plugin Files", "DisplayUnits", "TextUnit"));
            var files = plugDir.EnumerateFiles ();
            var plugId = Guid.NewGuid ();
            var plugFiles = 
                files
                    .Select (p => new PluginFileInfo (plugId, p))
                    .ToDictionary (p => p.FileId, p=> p);
            A.CallTo (() => fileMgr.GetFilesForPlugin (A<Guid>.Ignored)).Returns (plugFiles);
            tup.PluginId = Guid.NewGuid ();
            tup.Initialize (_appFac);

            A.CallTo (() => tempService.RegisterTemplate (A<string>.Ignored, A<string>.Ignored)).MustHaveHappened();
        }


    }
}

