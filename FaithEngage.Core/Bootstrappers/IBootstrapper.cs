using System;
using FaithEngage.Core.Containers;
using System.Collections.Generic;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Bootstrappers
{
	public interface IBootstrapper
	{
        BootPriority BootPriority { get; }
        void RegisterDependencies(IRegistrationService regService);
        void Execute(IAppFactory factory);
        void LoadBootstrappers (IBootList bootstrappers);
	}
}

