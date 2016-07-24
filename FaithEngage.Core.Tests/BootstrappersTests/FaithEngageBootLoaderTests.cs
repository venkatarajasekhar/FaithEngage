using System;
using NUnit.Framework;
using FaithEngage.Core.Containers;
using FakeItEasy;
using FaithEngage.Core.Factories;
using System.Collections.Generic;

namespace FaithEngage.Core.Bootstrappers
{
    [TestFixture]
    public class FaithEngageBootLoaderTests
    {
        private IContainer _container = A.Fake<IContainer> ();
        private IAppFactory _appFac = A.Fake<IAppFactory>();
        private IRegistrationService _rs = A.Fake<IRegistrationService> ();
        [Test]
        public void Execute_ActivatesAppFactory()
        {
            var feBooter = new FaithEngageBootLoader ();
            feBooter.Execute (_appFac);
            Assert.That (FEFactory.Get, Is.EqualTo (_appFac));
        }

        [Test]
        public void RegisterDependencies_RegistersDependencies ()
        {
            var feBooter = new FaithEngageBootLoader ();
            feBooter.RegisterDependencies (_rs);

            A.CallTo (() => _rs.Register<IAppFactory, AppFactory> (LifeCycle.Singleton)).MustHaveHappened();
        }

        [Test]
        public void LoadBootstrappers_AddsOtherBootstrappersToList()
        {
            var list = A.Fake<IBootList> ();
            var feBooter = new FaithEngageBootLoader ();

            feBooter.LoadBootstrappers (list);

            A.CallTo (list).Where(p=> p.Method.Name == "Load").WithAnyArguments().MustHaveHappened (Repeated.Exactly.Times (7));
        }


    }
}

