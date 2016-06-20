using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits.Factories;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core.DisplayUnits
{
	public class DisplayUnitBootstrapper : IBootstrapper
	{
		public void Execute(IContainer container)
		{
			
		}

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies(IContainer container)
		{
            container.Register<IDisplayUnitFactory, DisplayUnitFactory>(LifeCycle.Transient);
            container.Register<IDisplayUnitsRepoManager, DisplayUnitsRepoManager> (LifeCycle.Transient);
            container.Register<IDisplayUnitDtoFactory, DisplayUnitDtoFactory> (LifeCycle.Transient);
		}
	}
}

