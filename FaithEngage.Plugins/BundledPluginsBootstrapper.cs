using System;
using System.Collections.Generic;
using FaithEngage.Core;
using FaithEngage.Core.Containers;
using FaithEngage.Plugins.RazorTemplating;

namespace FaithEngage.Plugins
{
	public class BundledPluginsBootstrapper : IBootstrapper
	{

		public void Execute(IContainer container)
		{

		}

		public void LoadBootstrappers(IList<IBootstrapper> bootstrappers)
		{
			var templateBooter = new RazorTemplatingBootstrapper();

			bootstrappers.Add(templateBooter);

			templateBooter.LoadBootstrappers(bootstrappers);

		}

		public void RegisterDependencies(IContainer container)
		{

		}
	}
}

