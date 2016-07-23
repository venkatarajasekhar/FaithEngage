using System;
using FaithEngage.Core.Containers;
using System.Collections.Generic;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core
{
	public interface IBootstrapper
	{
        void RegisterDependencies(IRegistrationService regService);
        void Execute(IAppFactory factory);
        void LoadBootstrappers (IBootList bootstrappers);
	}
}

