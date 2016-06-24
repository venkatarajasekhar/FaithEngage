using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	public class DisplayUnitPluginBootstrapper : IBootstrapper
	{
		public void Execute(IContainer container)
		{
			var pluginContainer = container.Resolve<IDisplayUnitPluginContainer>();
			var repoManager = container.Resolve<IDisplayUnitPluginRepoManager>();
			foreach (var plugin in repoManager.GetAll()) {
                plugin.RegisterDependencies (container);
                plugin.Initialize (container);
                pluginContainer.Register (plugin);
			}
		}

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies(IContainer container)
		{
			container.Register<IDisplayUnitPluginContainer, DisplayUnitPluginContainer>(LifeCycle.Singleton);
			container.Register<IConverterFactory<DisplayUnitPlugin,DisplayUnitPluginDTO>, DisplayUnitPluginDtoFactory>(LifeCycle.Transient);
			container.Register<IDisplayUnitPluginFactory, DisplayUnitPluginFactory>(LifeCycle.Transient);
			container.Register<IDisplayUnitPluginRepoManager, DisplayUnitPluginRepoManager>(LifeCycle.Transient);
		}

	}
}

