using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using System.Linq;
using System.Reflection;

namespace FaithEngage.Core.Containers
{
    /// <summary>
	/// This is a home-spun inversion-of-control container that allows for dependency injection to work throughtout
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
	public class IocContainer : IContainer
    {
		public IocContainer ()
        {
            //On construction, registers the registration service used by this container.
			this.Register<IRegistrationService, RegistrationService> (LifeCycle.Singleton);
        }

		//The registry of all type registrations.
        private readonly List<RegisteredObject> _registry = new List<RegisteredObject>();

		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as a singleton.
		/// </summary>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		public void Register<Tabstract, Tconcrete> ()
        {
            Register<Tabstract,Tconcrete> (LifeCycle.Singleton);
        }
		/// <summary>
		/// Registers an interface/abstract class with a concrete implementation as transient
		/// 
		/// </summary>
		/// <param name="lifecycle">Whether the registered concrete class is to be registered as a singleton (i.e. a single
		/// instance shared accross the application) or as transient (i.e. a new object is created each time it is requested).</param>
		/// <typeparam name="Tabstract">The abstract class/interface to register</typeparam>
		/// <typeparam name="Tconcrete">The concrete implementation to register </typeparam>
		public void Register<Tabstract,Tconcrete>(LifeCycle lifecycle)
        {
            //Make sure that tconcrete is actually an implementation of tabstract. If not, throw exception
			if (!typeof (Tabstract).IsAssignableFrom (typeof (Tconcrete)))
                throw new InvalidTypeRelationshipException (typeof (Tabstract), typeof (Tconcrete));
            //Create a new registered object
			var ro = new RegisteredObject (typeof(Tabstract), typeof(Tconcrete), lifecycle);
            //Add it to the registry
			_registry.Add (ro);
        }

		/// <summary>
		/// Obtains an IRegistrationService that is connected to this container.
		/// </summary>
		/// <returns>The registration service.</returns>
		public IRegistrationService GetRegistrationService()
		{
            return this.Resolve<IRegistrationService> ();
		}

		/// <summary>
		/// Used in the event a concrete class being registered to itself, essentially enabling a singleton object.
		/// </summary>
		/// <returns>A registered object where TAbstract and TConcrete are the same</returns>
		/// <param name="typeToRegister">Type to register.</param>
		private RegisteredObject registerSelf(Type typeToRegister)
		{
			var ro = new RegisteredObject (typeToRegister, typeToRegister, LifeCycle.Singleton);
			_registry.Add (ro);
			return ro;
		}

		 /// <summary>
        /// Will provide a concrete implementation of the abstract class/interface type passed in. If the type parameter
		/// is a concrete class rather than abstract/interface, the IContainer will attempt to construct it, injecting
		/// any dependencies that might be requested.
        /// </summary>
        /// <typeparam name="T">The type desired.</typeparam>
        public TtypeToResolve Resolve<TtypeToResolve>()
        {
            return (TtypeToResolve)resolveObject (typeof(TtypeToResolve));
        }

        /// <summary>
        /// Resolves the type requested.
        /// </summary>
        /// <returns>The object of the requested type</returns>
        /// <param name="typeToResolve">Type to resolve.</param>
		private object resolveObject(Type typeToResolve)
        {
			//If IContainer is being requested, just return this.
			if (typeToResolve == typeof(IContainer))
				return this;
			//Find the regisered object for the specific type.
			var registeredObject = _registry.FirstOrDefault (o => o.AbtractType == typeToResolve);
			//If there are no registered objects in the registry for the requested type...
			if(registeredObject == null)
            {
				//Check if it's constructable...
				if (!typeToResolve.IsAbstract && !typeToResolve.IsInterface) {
					//If it is, register itself as a singleton.
					registeredObject = registerSelf (typeToResolve);
				} else {
					//Else, throw
					throw new TypeNotRegisteredException (typeToResolve);
				}
            }
            //Return a constructed instance of the type.
			return getInstance (registeredObject);
        }

		/// <summary>
		/// Obtains an instance of the given registered object, either by constructing it anew, or if it is a singleton
		/// and it has already been constructed, returning the instance of it.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="registeredObject">Registered object.</param>
        private object getInstance(RegisteredObject registeredObject)
        {
            //If the registered object is either a transient or hasn't yet been constructed...
			if(registeredObject.Instance == null ||
                registeredObject.LifeCycle == LifeCycle.Transient)
            {
                //Obtain the parameters needed for the constructor of the registered object
				var parameters = resolveConstructorParameters (registeredObject);
                //Create the instance of the registered object
				registeredObject.CreateInstance (parameters.ToArray ());
            }
            return registeredObject.Instance;
        }

		/// <summary>
		/// Resolves the constructor parameters, one at a time, for the given registered object.
		/// </summary>
		/// <returns>The constructor parameters.</returns>
		/// <param name="registeredObject">Registered object.</param>
        private IEnumerable<object> resolveConstructorParameters(RegisteredObject registeredObject)
        {
            ConstructorInfo constructorInfo = null;
            try {
                //Attempt to obtain the first constructor for the registered object.
				constructorInfo = registeredObject.ConcreteType.GetConstructors ().First ();
            } catch (InvalidOperationException) {
                throw new NoPublicConstructorsException ("Type has no public constructors: " + registeredObject.ConcreteType.Name);
            }
			//Loop through the constructor parameters...
            foreach(var parameter in constructorInfo.GetParameters())
            {
                //Resolve the constructor parameter
				yield return resolveObject (parameter.ParameterType);
            }
        }

		/// <summary>
		/// Deregisters the specified type from the container, regardless if the type is the abstract/interface type or 
		/// if the type is the concrete implementation.
		/// </summary>
		/// <returns>Number deregistered.</returns>
		/// <typeparam name="T">The abstract or concrete type to deregister.</typeparam>
		public int DeRegister<T>()
		{
			int num = 0;
			var typeToFind = typeof(T);
			num = _registry.RemoveAll(p => p.AbtractType.FullName == typeToFind.FullName || p.ConcreteType.FullName == typeToFind.FullName);
			return num;
		}
		/// <summary>
		/// Replaces the specified concrete implementation of an already registered class with a different implementation.
		/// If the abstract/interface type is not already registered, it will simply be registered.
		/// </summary>
		/// <param name="lifecycle">Lifecycle.</param>
		/// <typeparam name="Tabstract">The abstract/interface type</typeparam>
		/// <typeparam name="TnewConcrete">The NEW conrete implementation to be registered.</typeparam>
		public void Replace<Tabstract, TnewConcrete>(LifeCycle lifeCycle)
		{
			DeRegister<Tabstract>();
			Register<Tabstract, TnewConcrete>(lifeCycle);
		}
		/// <summary>
        /// Checks whether the specified abstract/interface type is currently registered.
        /// </summary>
        /// <returns><c>true</c>, if registered already, <c>false</c> otherwise.</returns>
        /// <typeparam name="Tabstract">The type to check</typeparam>
        public bool CheckRegistered<Tabstract> ()
        {
            var type = typeof (Tabstract);
            return _registry.Any (p => p.AbtractType == type);
        }
		/// <summary>
        /// Runs through all currently registered concrete types and checks their constructor dependencies, recursively,
		/// to ensure all required dependencies have been met. Any unregistered dependencies will be returned in the list
		/// of types.
        /// </summary>
        /// <returns>A list of type dependencies that are required by registered types but are not themselves currently registered.</returns>
        public IList<Type> CheckAllDependencies ()
        {
            var list = new List<Type> ();
            foreach(var obj in _registry)
            {
                checkDependencies (obj.ConcreteType, list);
            }
            return list;
        }

		/// <summary>
		/// Checks the dependencies of the specified type, adding any unregistered dependency types to the list.
		/// </summary>
		/// <param name="type">The type to check dependencies on</param>
		/// <param name="unregisteredTypes">The unregistered type dependencies list</param>
        private void checkDependencies(Type type, IList<Type> unregisteredTypes)
        {
            //If the list of unregistered types already contains this type, nothing futher is necessary.
			if (unregisteredTypes.Contains (type)) return;
			Type concreteType;
            //If the type is an interface or abstract
			if(type.IsAbstract || type.IsInterface)
            {
                //If the type is for IContainer, no futher checking is necessary.
				if (type == typeof (IContainer)) return;
                //Check the registry for the abstract type being registered
				var ro = _registry.FirstOrDefault (p => p.AbtractType == type);
                //get the concrete type or null
				concreteType = ro?.ConcreteType ?? null;
                if(concreteType == null) //If null...
                {
                    //Add it to the list and return.
					unregisteredTypes.Add (type);
                    return;
                }
            }
            else //The type is concrete, not abstract or an interface.
            {
                concreteType = type;
            }
			//If the list of unregistered types already contains the concrete type, nothing further is necessary
			if (unregisteredTypes.Contains (concreteType)) return;

			//Get the first constructor for the concrete type.
            var ctor = concreteType.GetConstructors ().FirstOrDefault ();
            if(ctor == null){//If it's null, return. It has no public constructors, and therefore no dependencies.
                return;
            }
            //Get the constructor parameters.
			var ctorParams = ctor.GetParameters ();
            foreach(var p in ctorParams) //For each constructor parameter...
            {
                //Check the dependencies on those constructor parameters
				checkDependencies (p.ParameterType, unregisteredTypes);
            }
			//If the registry contains any registered objects where the abstract or concrete type is this
			//concrete type, return. The dependency has been found.
			if (_registry.Any (p => p.AbtractType == type || p.ConcreteType == concreteType)) return;
            //If the type made it to this point, the type isn't registered and it should be added to the list. 
			unregisteredTypes.Add (type);
        }


    }
}

