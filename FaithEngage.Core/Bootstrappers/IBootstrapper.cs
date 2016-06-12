using System;
using FaithEngage.Core.Containers;
using System.Collections.Generic;

namespace FaithEngage.Core
{
	public interface IBootstrapper
	{
		void RegisterDependencies(IContainer container);
		void Execute(IContainer container);
        void LoadBootstrappers (IList<IBootstrapper> bootstrappers);
	}
}

