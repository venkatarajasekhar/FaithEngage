using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers;

namespace FaithEngage.Core
{
	public class FaithEngageBootLoader : IBootstrapper
	{
		public void Execute(IContainer container)
		{
			var pluginBooter = new PluginBootstrapper();
			pluginBooter.Execute(container);
		}

		public void RegisterDependencies(IContainer container)
		{
		}
	}
}

