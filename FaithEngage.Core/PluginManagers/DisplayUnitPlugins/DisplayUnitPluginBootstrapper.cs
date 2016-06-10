using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	internal class DisplayUnitPluginBootstrapper : IBootstrapper
	{
		public void Execute(IContainer container)
		{
			var pluginContainer = container.Resolve<IDisplayUnitPluginContainer>();
			var repoManager = container.Resolve<IDisplayUnitPluginRepoManager>();
			foreach (var plugin in repoManager.GetAll()) {
				pluginContainer.Register (plugin);
			}
		}

		public void RegisterDependencies(IContainer container)
		{
			container.Register<IDisplayUnitPluginContainer, DisplayUnitPluginContainer>(LifeCycle.Singleton);
			container.Register<IDisplayUnitPluginDtoFactory, DisplayUnitPluginDtoFactory>(LifeCycle.Transient);
			container.Register<IDisplayUnitPluginFactory, DisplayUnitPluginFactory>(LifeCycle.Transient);
			container.Register<IDisplayUnitPluginRepoManager, DisplayUnitPluginRepoManager>(LifeCycle.Transient);
		}

	}
}

