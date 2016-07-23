using System;
namespace FaithEngage.Core.Containers
{
	public interface IRegistrationService
	{
		void Register<Tabstract, Tconcrete>(LifeCycle lifeCycle);
		void Register<Tabstract, Tconcrete>();
	}
}

