using System;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core
{
	public interface IBootstrapper
	{
		void RegisterDependencies(IContainer container);
		void Execute(IContainer container);
	}
}

