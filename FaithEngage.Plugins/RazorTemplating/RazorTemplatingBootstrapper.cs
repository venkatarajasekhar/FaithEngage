using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.TemplatingService;

namespace FaithEngage.CorePlugins.RazorTemplating
{
	public class RazorTemplatingBootstrapper : IBootstrapper
	{
		public BootPriority BootPriority
		{
			get
			{
				return BootPriority.First;
			}
		}

		public void Execute(IAppFactory factory)
		{
		}

		public void LoadBootstrappers(IBootList bootstrappers)
		{
		}

		public void RegisterDependencies(IRegistrationService regService)
		{
			regService.Register<ITemplatingService, RazorTemplatingService>(LifeCycle.Singleton);
		}
	}
}
