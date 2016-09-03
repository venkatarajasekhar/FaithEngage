using System;
using NUnit.Framework;
using FaithEngage.Core.Config;
using FakeItEasy;
using System.IO;
using FaithEngage.Core.Tests;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
    public class PluginFileInfoDTOFactoryTests
    {
        private IConfigManager _config;

        [SetUp]
        public void Init()
        {
            _config = A.Fake<IConfigManager> ();
            A.CallTo (() => _config.PluginsFolderPath).Returns ("folder\\plugins");
        }

        [Test]
        public void Convert_FileInPluginsPath_ConvertsPluginFileInfo()
        {
            var guid = Guid.NewGuid ();
            var ds = Path.DirectorySeparatorChar;
            var file = new FileInfo ($"folder{ds}plugins{ds}{guid}{ds}file.txt");

            var pfile = new PluginFileInfo (guid, file);

            var fac = new PluginFileInfoDTOFactory (_config);

            var dto = fac.Convert (pfile);

            Assert.That (dto, Is.Not.Null);
            Assert.That (dto.RelativePath, Is.EqualTo("file.txt"));
            Assert.That (dto.Name, Is.EqualTo("file.txt"));
            Assert.That (dto.PluginId, Is.EqualTo(guid));
        }

        [Test]
        public void Convert_FileNotInPluginsPath_ThrowsFactoryException()
        {
            var guid = Guid.NewGuid ();
            var file = new FileInfo ("file.txt");

            var pfile = new PluginFileInfo (guid, file);

            var fac = new PluginFileInfoDTOFactory (_config);

            var e = TestHelpers.TryGetException(()=> fac.Convert (pfile));

            Assert.That (e, Is.Not.Null);
            Assert.That (e, Is.InstanceOf<FactoryException> ());

        }

    }
}

