using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Exceptions;
using NUnit.Framework;
using FaithEngage.Core.Tests;
using System;

namespace FaithEngage.Core.Containers
{
    [TestFixture]
    public class IocContainerTests
    {

        [Test]
        public void Register_ValidAbstract_ValidConcrete_NoException()
        {
            var container = new IocContainer ();
            container.Register<IDummy,Dummy_NoParameters> ();
        }
        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.InvalidTypeRelationshipException")]
        public void Register_InvalidTypeRelationship_InvalidTypeRelationshipException()
        {
            var container = new IocContainer();
            container.Register<IDummy,DisplayUnit> ();
        }

        [Test]
        public void Resolve_RegisteredType_ReturnsConcreteInstance()
        {
            var container = new IocContainer ();
            container.Register<IDummy,Dummy_NoParameters> ();

            var krp = container.Resolve<IDummy> ();

            Assert.That (krp, Is.InstanceOf (typeof(Dummy_NoParameters)));
            Assert.That (krp, Is.InstanceOf (typeof(IDummy)));
        }

        [Test]
        public void Resolve_RegisteredType_Transient_TwoDistinctInstances()
        {
            var container = new IocContainer ();
            container.Register<IDummy,Dummy_NoParameters> (LifeCycle.Transient);

            var dummy1 = container.Resolve<IDummy> ();
            var dummy2 = container.Resolve<IDummy> ();

            Assert.That (!Object.ReferenceEquals (dummy1, dummy2));
        }

        [Test]
        public void Resolve_RegisteredType_SingleTon_SharedInstance ()
        {
            var container = new IocContainer ();
            container.Register<IDummy, Dummy_NoParameters> (LifeCycle.Singleton);

            var dummy1 = container.Resolve<IDummy> ();
            var dummy2 = container.Resolve<IDummy> ();

            Assert.That (Object.ReferenceEquals (dummy1, dummy2));
        }

        [Test]
        public void Resolve_RegisteredType_With_Dependencies_Returns_ConcreteInstance()
        {
            var container = new IocContainer ();
            container.Register<IDummy,Dummy_CtorHasDependencies> ();
            container.Register<IDummy2,Dummy2_NoParameters> ();
            var dummy = container.Resolve<IDummy> ();

            Assert.That (dummy, Is.Not.Null);
            Assert.That (dummy, Is.InstanceOf<IDummy> ());
        }

        [Test]
        [ExpectedException(typeof(TypeNotRegisteredException))]
        public void Resolve_Registeredtype_WithUnregisteredDependencies_ThrowsException()
        {
            var container = new IocContainer ();
            container.Register<IDummy,Dummy_CtorHasDependencies> ();
            var dummy = container.Resolve<IDummy> ();
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.TypeNotRegisteredException")]
        public void Resolve_NotRegisteredtypeWithUnregisteredDependencies_TypeNotRegisteredException()
        {
            var container = new IocContainer ();
            var notRef = container.Resolve<IDummy> ();
        }

		[Test]
		public void Resolve_UnregisteredConcreteType_RegisteredDependencies_ReturnsInstance()
		{
			var container = new IocContainer ();
			container.Register<IDummy2,Dummy2_NoParameters> ();
			var dummy2 = container.Resolve<Dummy_CtorHasDependencies> ();

			Assert.That (dummy2, Is.Not.Null);
		}

		[Test]
		public void Resolve_TypeDependsOnIContainer_Resolves()
		{
			var container = new IocContainer ();
			var dummy = container.Resolve<Dummy_CtorDependsOnIContainer> ();

			Assert.That (dummy, Is.Not.Null);
		}
    }
}

