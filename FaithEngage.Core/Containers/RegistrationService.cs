using System;
namespace FaithEngage.Core.Containers
{
	public class RegistrationService : IRegistrationService
	{
		private readonly IContainer _container;
		public RegistrationService(IContainer container)
		{
			_container = container;
		}

		public void Register<Tabstract, Tconcrete>(LifeCycle lifeCycle)
		{
			_container.Register<Tabstract, Tconcrete>(lifeCycle);
		}

		public void Register<Tabstract, Tconcrete>()
		{
			_container.Register<Tabstract, Tconcrete>();
		}
	}
}

