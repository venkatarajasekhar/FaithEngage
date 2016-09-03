using System;
using NUnit.Framework;
using FaithEngage.Core.Config;
using FakeItEasy;
using System.IO;

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
        public void Convert_ConvertsPluginFileInfo()
        {
            
        }
    }
}

