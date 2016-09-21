using System;
namespace FaithEngage.Core.Containers
{
	/// <summary>
	/// The IRegistration service provides the ability to register an abstract/interface type with its concrete implemented
	/// type.
	/// </summary>
	public interface IRegistrationService
	{
		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as transient
		/// 
		/// </summary>
		/// <param name="lifecycle">Whether the registered concrete class is to be registered as a singleton (i.e. a single
		/// instance shared accross the application) or as transient (i.e. a new object is created each time it is requested).</param>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		void Register<Tabstract, Tconcrete>(LifeCycle lifeCycle);
		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as a singleton.
		/// </summary>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		void Register<Tabstract, Tconcrete>();
	}
}

