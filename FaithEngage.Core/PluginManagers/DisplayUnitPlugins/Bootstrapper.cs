using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	public class Bootstrapper : IBootstrapper
	{
		private readonly IDisplayUnitPluginContainer _container;
		private readonly IDisplayUnitPluginRepoManager _repoMgr;

		public Bootstrapper (IDisplayUnitPluginContainer pluginContainer, IDisplayUnitPluginRepoManager repoManager)
		{
			_container = pluginContainer;
			_repoMgr = repoManager;
		}

		public void Execute()
		{
			foreach (var plugin in _repoMgr.GetAll()) {
				_container.Register (plugin);
			}
		}
	}
}

