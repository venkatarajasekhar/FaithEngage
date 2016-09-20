using System;
using NUnit.Framework;
using FaithEngage.Core.Containers;
using FakeItEasy;
using FaithEngage.Core.Factories;
using System.Linq;

namespace FaithEngage.Core.Bootstrappers
{
    [TestFixture]
    public class BootListTests
    {
        private IContainer _container;
        private IBootstrapper _booter;
        private BootList _bootList;

        class dummyBooter1 : IBootstrapper
        {
            public BootPriority BootPriority {
                get {
                    return BootPriority.First;
                }
            }

            public void Execute (IAppFactory factory)
            {
                throw new NotImplementedException ();
            }

            public void LoadBootstrappers (IBootList bootstrappers)
            {
            }

            public void RegisterDependencies (IRegistrationService regService)
            {
                throw new NotImplementedException ();
            }
        }

        class dummyBooter2 : dummyBooter1{}

        class nonBooter{}



        [SetUp]
        public void init()
        {
            _container = A.Fake<IContainer> ();
            _booter = A.Fake<IBootstrapper> ();
            _bootList = new BootList (_container);
        }

        [Test]
        public void MissingDependencies_ReportsMissingDeps()
        {
            var missingDeps = _bootList.MissingDependencies;
            A.CallTo (() => _container.CheckAllDependencies ()).MustHaveHappened();
        }


        [Test]
        public void Add_AddsToList_CallsLoadBootstrappers()
        {
            _bootList.Add (_booter);
            Assert.That(_bootList.Count, Is.EqualTo(1));
            A.CallTo (() => _booter.LoadBootstrappers (_bootList)).MustHaveHappened();
        }

        [Test]
        public void Add_Multiples_OnlyAddsOnce ()
        {
            _bootList.Add (_booter);
            _bootList.Add (_booter);
            Assert.That (_bootList.Count, Is.EqualTo (1));
        }

        [Test]
        public void Load_ValidBootstrapper_AddsToBootList()
        {
            _bootList.Load<dummyBooter1> ();
            Assert.That (_bootList.Count == 1);
            Assert.That (_bootList.Count (p => p is dummyBooter1) == 1);
        }

        [Test]
        public void Load_MultipleWithDuplicates_ExcludesDuplicates()
        {
            _bootList.Load<dummyBooter1> ();
            _bootList.Load<dummyBooter2> ();
            _bootList.Load<dummyBooter1> ();

            Assert.That (_bootList.Count == 2);
        }

        [Test]
        public void RegisterAllDependencies_NoDepCheck_CallsRegisterDepenencies()
        {
            A.CallTo (() => _booter.BootPriority).Returns (BootPriority.Last);

            _bootList.Add (_booter);

            var log = _bootList.RegisterAllDependencies (false);
            Console.Write (log);

            A.CallTo (() => _booter.RegisterDependencies (A<IRegistrationService>.Ignored)).MustHaveHappened();
            A.CallTo (() => _container.CheckAllDependencies ()).MustNotHaveHappened();
            Assert.That (log, Is.Not.Null);
            Assert.That (log, Is.Not.EqualTo (""));
        }

        [Test]
        public void RegisterAllDependencies_DepCheck_ChecksDependencies()
        {
            A.CallTo (() => _booter.BootPriority).Returns (BootPriority.Last);

            _bootList.Add (_booter);

            var log = _bootList.RegisterAllDependencies (true);
            Console.Write (log);
            A.CallTo (() => _container.CheckAllDependencies ()).MustHaveHappened ();
        }

        [Test]
        public void ExecuteAllBootstrappers_ExcecutesBootstrappers()
        {
            _bootList.Add (_booter);
            _bootList.ExecuteAllBootstrappers ();

            A.CallTo (() => _booter.Execute (A<IAppFactory>.Ignored)).MustHaveHappened();
        }



    }
}

