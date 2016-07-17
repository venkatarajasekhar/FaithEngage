using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using System.Linq;

namespace FaithEngage.Core.Containers
{
    public class IocContainer : IContainer
    {
        public IocContainer ()
        {
        }

        private readonly List<RegisteredObject> _registry = new List<RegisteredObject>();

        public void Register<Tabstract, Tconcrete> ()
        {
            Register<Tabstract,Tconcrete> (LifeCycle.Singleton);
        }

        public void Register<Tabstract,Tconcrete>(LifeCycle lifecycle)
        {
            if (!typeof(Tabstract).IsAssignableFrom (typeof(Tconcrete)))
                throw new InvalidTypeRelationshipException (typeof(Tabstract), typeof(Tconcrete));
            var ro = new RegisteredObject (typeof(Tabstract), typeof(Tconcrete), lifecycle);
            _registry.Add (ro);
        }

		public IRegistrationService GetRegistrationService()
		{
			return this.Resolve<IRegistrationService>();
		}

		private RegisteredObject registerSelf(Type typeToRegister)
		{
			var ro = new RegisteredObject (typeToRegister, typeToRegister, LifeCycle.Singleton);
			_registry.Add (ro);
			return ro;
		}

        public TtypeToResolve Resolve<TtypeToResolve>()
        {
            return (TtypeToResolve)resolveObject (typeof(TtypeToResolve));
        }

        private object resolveObject(Type typeToResolve)
        {
			if (typeToResolve == typeof(IContainer))
				return this;
			var registeredObject = _registry.FirstOrDefault (o => o.AbtractType == typeToResolve);
			if(registeredObject == null)
            {
				if (!typeToResolve.IsAbstract && !typeToResolve.IsInterface) {
					registeredObject = registerSelf (typeToResolve);
				} else {
					throw new TypeNotRegisteredException (typeToResolve);
				}
            }
            return getInstance (registeredObject);
        }

        private object getInstance(RegisteredObject registeredObject)
        {
            if(registeredObject.Instance == null ||
                registeredObject.LifeCycle == LifeCycle.Transient)
            {
                var parameters = resolveConstructorParameters (registeredObject);
                registeredObject.CreateInstance (parameters.ToArray ());
            }
            return registeredObject.Instance;
        }

        private IEnumerable<object> resolveConstructorParameters(RegisteredObject registeredObject)
        {
			var constructorInfo = registeredObject.ConcreteType.GetConstructors ().First ();
            foreach(var parameter in constructorInfo.GetParameters())
            {
                yield return resolveObject (parameter.ParameterType);
            }
        }

		public int DeRegister<T>()
		{
			int num = 0;
			var typeToFind = typeof(T);
			num = _registry.RemoveAll(p => p.AbtractType.FullName == typeToFind.FullName || p.ConcreteType.FullName == typeToFind.FullName);
			return num;
		}

		public void Replace<Tabstract, TnewConcrete>(LifeCycle lifeCycle)
		{
			DeRegister<Tabstract>();
			Register<Tabstract, TnewConcrete>(lifeCycle);
		}

        public bool CheckRegistered<Tabstract> ()
        {
            var type = typeof (Tabstract);
            return _registry.Any (p => p.AbtractType == type);
        }

        public IList<Type> CheckAllDependencies ()
        {
            var list = new List<Type> ();
            foreach(var obj in _registry)
            {
                checkDependencies (obj.ConcreteType, list);
            }
            return list;
        }

        private void checkDependencies(Type type, IList<Type> unregisteredTypes)
        {
            Type concreteType;
            if(type.IsAbstract || type.IsInterface)
            {
                if (type == typeof (IContainer)) return;
                var ro = _registry.FirstOrDefault (p => p.AbtractType == type);
                concreteType = ro?.ConcreteType ?? null;
                if(concreteType == null)
                {
                    if (unregisteredTypes.Contains (type)) return;
                    unregisteredTypes.Add (type);
                    return;
                }
            }
            else
            {
                concreteType = type;
            }

            var ctor = concreteType.GetConstructors ().FirstOrDefault ();
            if(ctor == null){
                return;
            }
            var ctorParams = ctor.GetParameters ();
            foreach(var p in ctorParams)
            {
                checkDependencies (p.ParameterType, unregisteredTypes);
            }
            if (unregisteredTypes.Contains (concreteType)) return;
            if (_registry.Any (p => p.AbtractType == type || p.ConcreteType == concreteType)) return;
            unregisteredTypes.Add (type);
        }


    }
}

