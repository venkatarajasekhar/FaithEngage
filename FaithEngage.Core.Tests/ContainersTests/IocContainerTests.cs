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

		[Test]
		public void DeRegister_AbstractType_RemovesFromContainer_ReturnsNumber() {
			var container = new IocContainer();
			container.Register<IDummy, Dummy_NoParameters>();
			var resolved = container.Resolve<IDummy>();
			var num = container.DeRegister<IDummy>();
			var e = TestHelpers.TryGetException(() => container.Resolve<IDummy>());

			Assert.That(resolved, Is.Not.Null);
			Assert.That(num == 1);
			Assert.That(e, Is.InstanceOf<TypeNotRegisteredException>());
		}

		[Test]
		public void DeRegister_ConcreteType_RemovesFromContainer_ReturnsNumber() {
			var container = new IocContainer();
			container.Register<IDummy, Dummy_NoParameters>();
			var resolved = container.Resolve<IDummy>();
			var num = container.DeRegister<Dummy_NoParameters>();
			var e = TestHelpers.TryGetException(() => container.Resolve<IDummy>());

			Assert.That(resolved, Is.Not.Null);
			Assert.That(num == 1);
			Assert.That(e, Is.InstanceOf<TypeNotRegisteredException>());
		}

		[Test]
		public void DeRegister_UnknownType_ReturnsZero() {
			var container = new IocContainer();
			var num = container.DeRegister<Dummy_NoParameters>();
			Assert.That(num == 0);
		}

		[Test]
		public void Replace_ExistingAbstract_ValidNewConcrete_ReplacesDependency() {
			var container = new IocContainer();
			container.Register<IDummy, Dummy_NoParameters>();
			var resolvedDummy1 = container.Resolve<IDummy>();
			container.Replace<IDummy, Dummy_NoParameters2>(LifeCycle.Transient);
			var resolvedDummy2 = container.Resolve<IDummy>();
			Assert.That(resolvedDummy1, Is.InstanceOf<Dummy_NoParameters>());
			Assert.That(resolvedDummy2, Is.InstanceOf<Dummy_NoParameters2>());
		}

		[Test]
		public void Replace_NonExistingAbstract_RegistersDependency() {
			var container = new IocContainer();
			container.Replace<IDummy, Dummy_NoParameters>(LifeCycle.Transient);
			var resolvedDummy = container.Resolve<IDummy>();
			Assert.That(resolvedDummy, Is.InstanceOf<Dummy_NoParameters>());
		}

    }
}

