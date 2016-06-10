using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginBootstrapper :IBootstrapper
	{
		public void Execute(IContainer container)
		{
			var duBooter = new DisplayUnitPluginBootstrapper();
			duBooter.Execute(container);
		}

		public void RegisterDependencies(IContainer container)
		{
		}
	}
}

