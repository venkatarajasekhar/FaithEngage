using System;
using System.Collections.Generic;
using FaithEngage.Core;
using FaithEngage.Core.Containers;
using FaithEngage.Core.TemplatingService;

namespace FaithEngage.Plugins.RazorTemplating
{
	public class RazorTemplatingBootstrapper : IBootstrapper
	{
		public void Execute(IContainer container)
		{
		}

		public void LoadBootstrappers(IList<IBootstrapper> bootstrappers)
		{

		}

		public void RegisterDependencies(IContainer container)
		{
			container.Register<ITemplatingService, RazorTemplatingService>(LifeCycle.Transient);
		}
	}
}

