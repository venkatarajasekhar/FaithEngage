using System;
using System.Collections.Generic;
using System.IO;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.Interfaces;
using FakeItEasy;
using NUnit.Framework;
namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
{
    public class TextUnitBootstrapperTests
    {

		private IAppFactory _fac;
		private IPluginManager _plugMgr;

		[SetUp]
		public void Init()
		{
			_fac = A.Fake<IAppFactory>();
			_plugMgr = A.Fake<IPluginManager>();
			A.CallTo(() => _fac.PluginManager).Returns(_plugMgr);
		}

        [Test]
        public void Execute_TextUnitPluginNotRegistered_Installs()
        {
			A.CallTo(() => _plugMgr.CheckRegistered<TextUnitPlugin>()).Returns(false);
			var booter = new TextUnitBootstrapper();
			booter.Execute(_fac);
			A.CallTo(() => _plugMgr.Install<TextUnitPlugin>(A<List<FileInfo>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Execute_TextUnitPluginRegistered_DoesNotInstall ()
        {
            A.CallTo(() => _plugMgr.CheckRegistered<TextUnitPlugin>()).Returns(true);
			var booter = new TextUnitBootstrapper();
			booter.Execute(_fac);
			A.CallTo(() => _plugMgr.Install<TextUnitPlugin>(A<List<FileInfo>>.Ignored)).MustNotHaveHappened();
        }
    }
}

