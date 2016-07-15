using System;
namespace FaithEngage.Core.Containers
{
	public class RegistrationService : IRegistrationService
	{
		private readonly IContainer _container;
		internal RegistrationService(IContainer container)
		{
			_container = container;
		}

		public void RegisterDependency<Tabstract, Tconcrete>(LifeCycle lifeCycle)
		{
			_container.Register<Tabstract, Tconcrete>(lifeCycle);
		}

		public void RegisterDependency<Tabstract, Tconcrete>()
		{
			_container.Register<Tabstract, Tconcrete>();
		}
	}
}

