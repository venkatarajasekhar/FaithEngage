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

			public DisplayUnitPlugin GetById(Guid id)
			{
				throw new NotImplementedException();
			}

			public Guid RegisterNew(DisplayUnitPlugin plugin)
			{
				throw new NotImplementedException();
			}

			public void UninstallPlugin(Guid id)
			{
				throw new NotImplementedException();
			}

			public void UpdatePlugin(DisplayUnitPlugin plugin)
			{
				throw new NotImplementedException();
			}
		}



		[Test]
		public void TestBootLoader_FakePluginRepoMgr()
		{

			var initializer = new Initializer();
			var booter = initializer.GetBootstrapper();
            var container = initializer.GetContainer ();



			var booters = new List<Core.IBootstrapper>();
            Console.WriteLine ("Loading Bootstrappers...");
            booter.LoadBootstrappers(booters);
            Console.WriteLine ("Registering Dependencies...");
			foreach (var boot in booters)
			{
                Console.WriteLine($"--Registering {boot.GetType().Name}.");
				Console.WriteLine (boot);
                boot.RegisterDependencies(container);
			}

			container.Replace<IDisplayUnitPluginRepoManager, dummyPluginRepoMgr>(LifeCycle.Singleton);

            Console.WriteLine ("Executing Booters...");
			foreach (var boot in booters)
			{
				Console.WriteLine($"--Executing on {boot.GetType().Name}.");
				boot.Execute (container);
			}
		}
	}
}

