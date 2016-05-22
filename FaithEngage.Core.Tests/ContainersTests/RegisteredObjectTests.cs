using NUnit.Framework;
using FaithEngage.Core.Tests;

namespace FaithEngage.Core.Containers
{
    [TestFixture]
    public class RegisteredObjectTests
    {

        private RegisteredObject ro_noParams;
        private RegisteredObject ro_oneParam;

        [TestFixtureSetUp]
        public void Ctor_ValidRegisteredObject()
        {
            ro_noParams = new RegisteredObject (typeof(IDummy), typeof(Dummy_NoParameters), LifeCycle.Singleton);
            ro_oneParam = new RegisteredObject (typeof(IDummy), typeof(Dummy_OneParam), LifeCycle.Singleton);
        }


        [Test]
        public void CreateInstance_NoParameters_CreatesConcreteInstance()
        {
            object[] parameters = new object[]{ };
            ro_noParams.CreateInstance (parameters);

            Assert.That (ro_noParams.Instance, Is.InstanceOf (typeof(Dummy_NoParameters)));
            Assert.That (ro_noParams.Instance, Is.Not.Null);
        }

        [Test]
        public void CreateInstance_ValidParameters_CreatesConcreteInstance()
        {
            object[] parameters = new object[]{"message" };
            ro_oneParam.CreateInstance (parameters);

            Assert.That (ro_oneParam.Instance, Is.InstanceOf (typeof(Dummy_OneParam)));
            Assert.That (ro_oneParam.Instance, Is.Not.Null);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.RegisteredObjectInstantiationException")]
        public void CreateInstance_AbstractConcreteType_RegisteredObjectInstantiationException()
        {
            object[] parameters = new object[]{};
            var ro = new RegisteredObject(typeof(IDummy),typeof(AbstractDummy_NoParams),LifeCycle.Singleton);
            ro.CreateInstance (parameters);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.RegisteredObjectInstantiationException")]
        public void CreateInstance_InvalidParameters_RegisteredObjectInstantiationException()
        {
            object[] parameters = new object[]{};
            ro_oneParam.CreateInstance (parameters);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.RegisteredObjectInstantiationException")]
        public void CreateInstance_ConstructorThrowsException_RegisteredObjectInstantiationException()
        {
            object[] parameters = new object[]{};
            var ro = new RegisteredObject(typeof(IDummy),typeof(Dummy_CtorThrowsException),LifeCycle.Singleton);
            ro.CreateInstance (parameters);
        }

    }
}
