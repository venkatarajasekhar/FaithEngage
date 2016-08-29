using System;
using NUnit.Framework;
using FaithEngage.Core.Config;
using FakeItEasy;
using System.IO;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
    public class PluginFileInfoFactoryTests
    {
        private IConfigManager _config;

        [SetUp]
        public void Init(){
            _config = A.Fake<IConfigManager> ();
            A.CallTo (() => _config.PluginsFolderPath).Returns ("folder\\plugins");
            A.CallTo (() => _config.TempFolderPath).Returns ("folder/temp");
        }

        [Test]
        public void Ctor_PopulatesAndCreatesFolders(){
            var expectedP = Path.Combine ("folder", "plugins");
            var expectedT = Path.Combine ("folder", "temp");

            if (Directory.Exists (expectedP)) Directory.Delete (expectedP);
            if (Directory.Exists (expectedT)) Directory.Delete (expectedT);

            Assert.That (Directory.Exists (expectedP), Is.Not.True);
            Assert.That (Directory.Exists (expectedT), Is.Not.True);

            var fac = new PluginFileInfoFactory (_config);

            Assert.That (fac.PluginsFolder.FullName.Contains (expectedP));
            Assert.That (fac.TempFolder.FullName.Contains (expectedT));
            fac.PluginsFolder.Refresh ();
            fac.TempFolder.Refresh ();
            Assert.That (fac.PluginsFolder.Exists);
            Assert.That (fac.TempFolder.Exists);

            fac.PluginsFolder.Delete ();
            fac.TempFolder.Delete ();
        }

        [Test]
        public void Convert_ExistantFileValidDTO_CreatesPluginFileInfo()
        {
            var plugId = Guid.NewGuid ();

            var fac = new PluginFileInfoFactory (_config);

            fac.PluginsFolder.CreateSubdirectory(plugId.ToString());

            var dto = new PluginFileInfoDTO () {
                Name = "TESTFILE.txt", FileId = Guid.NewGuid (), PluginId = plugId, RelativePath = "otherFolder\\TESTFILE.txt"
            };

            var newDir = Path.Combine (fac.PluginsFolder.FullName, plugId.ToString(), "otherFolder");
            Directory.CreateDirectory (newDir);

            var newPath = Path.Combine (newDir,"TESTFILE.txt");

            File.WriteAllText (newPath, "This is my test.");

            Assert.That (File.Exists(newPath));

            var pfileInfo = fac.Convert (dto);

            Assert.That (pfileInfo, Is.Not.Null);
            Assert.That (pfileInfo.FileId, Is.EqualTo(dto.FileId));
            Assert.That (pfileInfo.FileInfo.Exists);
            Assert.That (pfileInfo.PluginId, Is.EqualTo (plugId));

            Directory.Delete (newDir, true);
        }
    }
}

