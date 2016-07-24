using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits.Factories;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.Bootstrappers;

namespace FaithEngage.Core.DisplayUnits
{
	public class DisplayUnitBootstrapper : IBootstrapper
	{

        public void Execute(IAppFactory container)
		{
			
		}

        public void LoadBootstrappers (IBootList bootstrappers)
        {
        }

        public void RegisterDependencies(IRegistrationService rs)
		{
            rs.Register<IDisplayUnitFactory, DisplayUnitFactory>(LifeCycle.Transient);
            rs.Register<IDisplayUnitsRepoManager, DisplayUnitsRepoManager> (LifeCycle.Transient);
            rs.Register<IConverterFactory<DisplayUnit,DisplayUnitDTO>, DisplayUnitDtoFactory> (LifeCycle.Transient);
		}
	}
}

