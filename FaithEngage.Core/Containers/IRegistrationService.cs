using System;
namespace FaithEngage.Core.Containers
{
	public interface IRegistrationService
	{
		void RegisterDependency<Tabstract, Tconcrete>(LifeCycle lifeCycle);

		void RegisterDependency<Tabstract, Tconcrete>();
	}
}

