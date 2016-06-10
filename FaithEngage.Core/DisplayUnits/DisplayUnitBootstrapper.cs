using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits.Interfaces;

namespace FaithEngage.Core.DisplayUnits
{
	public class DisplayUnitBootstrapper : IBootstrapper
	{
		public void Execute(IContainer container)
		{
			
		}

		public void RegisterDependencies(IContainer container)
		{
			container.Register<IDisplayUnitFactory, DisplayUnitFactory>(LifeCycle.Singleton);
		}
	}
}

