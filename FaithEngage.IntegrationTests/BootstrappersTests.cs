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

namespace FaithEngage.IntegrationTests
{
    [TestFixture]
	public class BootstrappersTests
	{
		
		class dummyPluginRepoMgr : IDisplayUnitPluginRepoManager
		{
			public IEnumerable<DisplayUnitPlugin> GetAll()
			{
				DisplayUnitPlugin plugin = A.Fake<DisplayUnitPlugin>();
				var plugins = A.Fake<IEnumerable<DisplayUnitPlugin>>();
				A.CallTo(() => plugins.GetEnumerator().Current).Returns(plugin);
				return plugins;
			}

			public IDictionary<Guid, Plugin> GetAllPlugins()
			{
				throw new NotImplementedException();
			}

			public DisplayUnitPlugin GetById(Guid id)
			{
				throw new NotImplementedException();
			}

			public void RegisterNew(Plugin plugin, Guid pluginId)
			{
				throw new NotImplementedException();
			}

			public void UninstallPlugin(Guid id)
			{
				throw new NotImplementedException();
			}

			public void UpdatePlugin(Plugin plugin)
			{
				throw new NotImplementedException();
			}
		}



		[Test]
		public void TestBootLoader_FakePluginRepoMgr()
		{

			var initializer = new Initializer();
            Console.WriteLine ("Loading Bootstrappers...");
            var bootlist = initializer.LoadedBootList;
            foreach(var booter in bootlist){
                Console.WriteLine ($"--{booter.GetType ().Name}");
            }

            Console.WriteLine ("Registering Dependencies...");
            var log = bootlist.RegisterAllDependencies (true);
            Console.Write (log);

            initializer.Container.Replace<IDisplayUnitPluginRepoManager, dummyPluginRepoMgr>(LifeCycle.Singleton);

            log = bootlist.ExecuteAllBootstrappers ();
            Console.Write (log);
		}
	}
}

