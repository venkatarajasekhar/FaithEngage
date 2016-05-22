using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Exceptions;
using NUnit.Framework;
using FaithEngage.Core.Tests;

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
        public void Resolve_RegisteredType_Returns_ConcreteInstance()
        {
            var container = new IocContainer ();
            container.Register<IDummy,Dummy_NoParameters> ();

            var krp = container.Resolve<IDummy> ();

            Assert.That (krp, Is.InstanceOf (typeof(Dummy_NoParameters)));
            Assert.That (krp, Is.InstanceOf (typeof(IDummy)));
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
        public void Resolve_NotRegisteredtype_TypeNotRegisteredException()
        {
            var container = new IocContainer ();
            var notRef = container.Resolve<IDummy> ();
        }



    }
}

