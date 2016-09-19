using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.CorePlugins.DisplayUnits;
using FaithEngage.CorePlugins.RazorTemplating;

namespace FaithEngage.CorePlugins
{
	public class CorePluginsBootstrapper : IBootstrapper
	{
		public BootPriority BootPriority
		{
			get
			{
				return BootPriority.Normal;
			}
		}

		public void Execute(IAppFactory factory)
		{
		}

		public void LoadBootstrappers(IBootList bootstrappers)
		{
			bootstrappers.Load<DisplayUnitsBootstrapper>();
			bootstrappers.Load<RazorTemplatingBootstrapper>();
		}

		public void RegisterDependencies(IRegistrationService regService)
		{
			
		}
	}
}
