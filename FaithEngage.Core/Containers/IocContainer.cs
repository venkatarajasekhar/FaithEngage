﻿using System;
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
			var num = DeRegister<Tabstract>();
			if (num == 0) throw new TypeNotRegisteredException(typeof(Tabstract));
			Register<Tabstract, TnewConcrete>(lifeCycle);
		}
	}
}

