using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginBootstrapperTests
	{
		[Test]
		public void Execute_InitializesPlugins()
		{
			var fac = A.Fake<IAppFactory>();
			var plugMgr = A.Fake<IPluginManager>();
			A.CallTo(() => fac.PluginManager).Returns(plugMgr);

			var booter = new PluginBootstrapper();
			booter.Execute(fac);

			A.CallTo(() => plugMgr.InitializeAllPlugins()).MustHaveHappened();
		}
	}
}

