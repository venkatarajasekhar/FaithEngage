using NUnit.Framework;
using System;
using FaithEngage.Facade;
using FaithEngage.Core;
using System.Collections.Generic;
using FakeItEasy;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.Config;

namespace FaithEngage.IntegrationTests
{
    [TestFixture]
	public class BootstrappersTests
	{

        [Test]
		public void TestBootLoader_FakePluginRepoMgr()
		{
            Assert.Ignore ("Needed dependencies not created yet.");
            var initializer = new Initializer();
            Console.WriteLine ("Loading Bootstrappers...");
            var bootlist = initializer.LoadedBootList;
            foreach(var booter in bootlist){
                Console.WriteLine ($"--{booter.GetType ().Name}");
            }

            Console.WriteLine ("Registering Dependencies...");
            var log = bootlist.RegisterAllDependencies (true);
            Console.Write (log);

            log = bootlist.ExecuteAllBootstrappers ();
            Console.Write (log);
		}
	}
}

