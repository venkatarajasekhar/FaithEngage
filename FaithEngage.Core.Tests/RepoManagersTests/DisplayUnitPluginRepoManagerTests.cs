using System;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Tests;
using FakeItEasy;
using NUnit.Framework;
using System.Linq;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.RepoManagers
{
	[TestFixture]
	public class DisplayUnitPluginRepoManagerTests
	{
		private IPluginRepository _repo;
		private IDisplayUnitPluginFactory _fac;
		private IPluginRepoManager _pRepoMgr;
		private const string VALID_STRING = "VALID";
		private const string INVALID_STRING = "INVALID";
		private Guid VALID_GUID = Guid.NewGuid();
		private DisplayUnitPlugin _plgn;
        private DisplayUnitPluginRepoManager mgr;

        private class dumbPlug : Plugin
        {
            public override string PluginName {
                get {
                    throw new NotImplementedException ();
                }
            }

            public override int [] PluginVersion {
                get {
                    throw new NotImplementedException ();
                }
            }

            public override void Initialize (IAppFactory FEFactory)
            {
                throw new NotImplementedException ();
            }

            public override void Install (IAppFactory FEFactory)
            {
                throw new NotImplementedException ();
            }

            public override void RegisterDependencies (IRegistrationService regService)
            {
                throw new NotImplementedException ();
            }

            public override void Uninstall (IAppFactory FEFactory)
            {
                throw new NotImplementedException ();
            }
        }



        [SetUp]
		public void Init()
		{
			_repo = A.Fake<IPluginRepository>();
			_fac = A.Fake<IDisplayUnitPluginFactory>();
			_plgn = A.Fake<DisplayUnitPlugin>();
			_pRepoMgr = A.Fake<IPluginRepoManager>();
			mgr = new DisplayUnitPluginRepoManager (_fac, _pRepoMgr, _repo);
		}

        [Test]
        public void GetAll_ReturnsPluginsIEnumerable()
        {
            var dtos = Enumerable.Repeat (new PluginDTO (), 5).ToList();
			A.CallTo (() => _repo.GetAll(PluginTypeEnum.DisplayUnit)).Returns (dtos);
            var duPlugins = A.CollectionOfFake<DisplayUnitPlugin> (5);
            A.CallTo (() => _fac.LoadPluginsFromDtos (dtos)).Returns (duPlugins);

            var plugins = mgr.GetAll ();

            Assert.That (plugins, Is.EqualTo (duPlugins));
        }

        [Test]
        public void GetAll_RepoThrowsException_ThrowsRepoException()
        {
            A.CallTo (() => _repo.GetAll (PluginTypeEnum.DisplayUnit)).Throws<Exception>();
            var e = TestHelpers.TryGetException (() => mgr.GetAll ());
            Assert.That (e, Is.InstanceOf (typeof (RepositoryException)));
        }
        [Test]
        public void GetById_ValidId_ReturnsPlugin()
        {
            var dto = new PluginDTO ();
            var plugin = A.Fake<DisplayUnitPlugin> ();
            A.CallTo (() => _repo.GetById (VALID_GUID)).Returns (dto);
            A.CallTo (() => _fac.LoadPluginFromDto (dto)).Returns (plugin);

            var duPlugin = mgr.GetById (VALID_GUID);

            Assert.That (duPlugin, Is.EqualTo (plugin));
        }
        [Test]
        public void GetById_EmptyGuid_ThrowsInvalidIdException()
        {
            var empty = Guid.Empty;
            var e = TestHelpers.TryGetException (() => mgr.GetById (empty));
            Assert.That (e, Is.InstanceOf (typeof (InvalidIdException)));
        }

        [Test]
        public void GetById_RepoThrowsException_ThrowsRepoException()
        {
            A.CallTo (() => _repo.GetById (VALID_GUID)).Throws<Exception>();
            var e = TestHelpers.TryGetException (() => mgr.GetById (VALID_GUID));
            Assert.That (e, Is.InstanceOf (typeof (RepositoryException)));
        }

        [Test]
        public void GetById_InvalidId_ThrowsInvalidIdException()
        {
            A.CallTo (() => _repo.GetById (VALID_GUID)).Throws<InvalidIdException>();
            var e = TestHelpers.TryGetException (() => mgr.GetById (VALID_GUID));
            Assert.That (e, Is.InstanceOf (typeof (InvalidIdException)));
        }

        [Test]
        public void CheckRegistered_RegisteredId_ReturnsTrue()
        {
            A.CallTo (() => _pRepoMgr.CheckRegistered (VALID_GUID)).Returns (true);
            IDisplayUnitPluginRepoManager repo = mgr;

            var regd = repo.CheckRegistered (VALID_GUID);
            Assert.That (regd);
        }

        [Test]
        public void CheckRegistered_UnRegisteredId_ReturnsFalse ()
        {
            A.CallTo (() => _pRepoMgr.CheckRegistered (VALID_GUID)).Returns (false);
            IDisplayUnitPluginRepoManager repo = mgr;

            var regd = repo.CheckRegistered (VALID_GUID);
            Assert.That (!regd);
        }

        [Test]
        public void CheckRegistered_RegisteredType_ReturnsTrue ()
        {
            
            A.CallTo (() => _pRepoMgr.CheckRegistered<dumbPlug>()).Returns (true);
            IDisplayUnitPluginRepoManager repo = mgr;

            var regd = repo.CheckRegistered<dumbPlug>();
            Assert.That (regd);
        }

        [Test]
        public void CheckRegistered_UnRegisteredType_ReturnsFalse ()
        {
            A.CallTo (() => _pRepoMgr.CheckRegistered<dumbPlug> ()).Returns (false);
            IDisplayUnitPluginRepoManager repo = mgr;

            var regd = repo.CheckRegistered<dumbPlug> ();
            Assert.That (!regd);
        }
	}
}

