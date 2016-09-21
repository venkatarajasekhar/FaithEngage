using System;
namespace FaithEngage.Core.Containers
{
	/// <summary>
	/// This is the implementation of the IRegistrationService used by the IocContainer class. It provides easy
	/// access to the IocContainer's Register() method, without providing other access to the container's other methods.
	/// This allows plugins and external code to register dependencies without having access to other dependencies.
	/// </summary>
	public class RegistrationService : IRegistrationService
	{
		private readonly IContainer _container;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Containers.RegistrationService"/> class.
		/// </summary>
		/// <param name="container">Container.</param>
		public RegistrationService(IContainer container)
		{
			//Encapsulate the IContainer
			_container = container;
		}

		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as transient
		/// 
		/// </summary>
		/// <param name="lifecycle">Whether the registered concrete class is to be registered as a singleton (i.e. a single
		/// instance shared accross the application) or as transient (i.e. a new object is created each time it is requested).</param>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		public void Register<Tabstract, Tconcrete>(LifeCycle lifeCycle)
		{
			_container.Register<Tabstract, Tconcrete>(lifeCycle);
		}
		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as a singleton.
		/// </summary>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		public void Register<Tabstract, Tconcrete>()
		{
			_container.Register<Tabstract, Tconcrete>();
		}
	}
}

