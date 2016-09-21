using System;
using System.Collections.Generic;

namespace FaithEngage.Core.Containers
{
    /// <summary>
	/// The IContainer is a home-spun inversion-of-control container that allows for dependency injection to work throughtout
	/// the application. The general process of use is that abstract classes/interfaces are registered to correspond with concrete
	/// implementations. Then, when an instance of a said abstract class/interface is requested (i.e. resolved), an instance
	/// of its implementation is provided as registered.
	/// 
	/// All constructor dependencies are injected as needed, so that the entire object dependency heirarchy is available
	/// at construction.
	/// 
	/// Dependencies can be registered with a singleton lifecycle or a transient one. Singleton dependencies stored and 
	/// shared across every request for one, while transient dependencies are constructed each time they are requested.
    /// </summary>
	public interface IContainer
    {
        /// <summary>
        /// Will provide a concrete implementation of the abstract class/interface type passed in. If the type parameter
		/// is a concrete class rather than abstract/interface, the IContainer will attempt to construct it, injecting
		/// any dependencies that might be requested.
        /// </summary>
        /// <typeparam name="T">The interface/abstract class desired.</typeparam>
		T Resolve<T>();
		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as a singleton.
		/// </summary>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		void Register<Tabstract, Tconcrete> ();
		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as transient
		/// 
		/// </summary>
		/// <param name="lifecycle">Whether the registered concrete class is to be registered as a singleton (i.e. a single
		/// instance shared accross the application) or as transient (i.e. a new object is created each time it is requested).</param>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		void Register<Tabstract, Tconcrete> (LifeCycle lifecycle);
		/// <summary>
		/// Deregisters the specified type from the container, regardless if the type is the abstract/interface type or 
		/// if the type is the concrete implementation.
		/// </summary>
		/// <returns>Number deregistered.</returns>
		/// <typeparam name="T">The abstract or concrete type to deregister.</typeparam>
		int DeRegister<T>();
		/// <summary>
		/// Replaces the specified concrete implementation of an already registered class with a different implementation.
		/// If the abstract/interface type is not already registered, it will simply be registered.
		/// </summary>
		/// <param name="lifecycle">Lifecycle.</param>
		/// <typeparam name="Tabstract">The abstract/interface type</typeparam>
		/// <typeparam name="TnewConcrete">The NEW conrete implementation to be registered.</typeparam>
		void Replace<Tabstract, TnewConcrete>(LifeCycle lifecycle);
		/// <summary>
		/// Obtains an IRegistrationService that is connected to this container.
		/// </summary>
		/// <returns>The registration service.</returns>
		IRegistrationService GetRegistrationService();
        /// <summary>
        /// Checks whether the specified abstract/interface type is currently registered.
        /// </summary>
        /// <returns><c>true</c>, if registered already, <c>false</c> otherwise.</returns>
        /// <typeparam name="Tabstract">The type to check</typeparam>
		bool CheckRegistered<Tabstract> ();
        /// <summary>
        /// Runs through all currently registered concrete types and checks their constructor dependencies, recursively,
		/// to ensure all required dependencies have been met. Any unregistered dependencies will be returned in the list
		/// of types.
        /// </summary>
        /// <returns>A list of type dependencies that are required by registered types but are not themselves currently registered.</returns>
		IList<Type> CheckAllDependencies ();
    }
}

