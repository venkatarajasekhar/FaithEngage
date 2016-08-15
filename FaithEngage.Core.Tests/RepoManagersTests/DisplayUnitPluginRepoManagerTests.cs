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

namespace FaithEngage.Core.RepoManagers
{
	[TestFixture]
	public class DisplayUnitPluginRepoManagerTests
	{
		private IPluginRepository _repo;
		private IDisplayUnitPluginFactory _fac;
		private IConverterFactory<Plugin,PluginDTO> _dtoFac;
		private const string VALID_STRING = "VALID";
		private const string INVALID_STRING = "INVALID";
		private Guid VALID_GUID = Guid.NewGuid();
		private DisplayUnitPlugin _plgn;
        private DisplayUnitPluginRepoManager mgr;

		[SetUp]
		public void Init()
		{
			_repo = A.Fake<IPluginRepository>();
			_fac = A.Fake<IDisplayUnitPluginFactory>();
			_dtoFac = A.Fake<IConverterFactory<Plugin,PluginDTO>>();
			_plgn = A.Fake<DisplayUnitPlugin>();
            mgr = new DisplayUnitPluginRepoManager (_fac, _repo, _dtoFac);
		}

        [Test]
		public void RegisterNew_ValidPlugin_RegistersAndReturnsId()
		{
			var dto = new PluginDTO()
			{
				AssemblyLocation = VALID_STRING,
				FullName = VALID_STRING,
				Id = VALID_GUID,
				PluginName = VALID_STRING
			};
			A.CallTo(() => _dtoFac.Convert(_plgn)).Returns(dto);
			A.CallTo(() => _repo.Register(A<PluginDTO>.Ignored)).Returns(VALID_GUID);

			var id = mgr.RegisterNew(_plgn);

			Assert.That(id, Is.EqualTo(VALID_GUID));
			Assert.That(_plgn.PluginId.HasValue);
		}

		[Test]
		public void RegisterNew_InvalidPlugin_Throws()
		{
			A.CallTo(() => _dtoFac.Convert(_plgn)).Throws<PluginIsMissingNecessaryInfoException>();
            var e = TestHelpers.TryGetException (() => mgr.RegisterNew (_plgn));
            Assert.That (e, Is.Not.Null);
            Assert.That (e, Is.InstanceOf (typeof (PluginIsMissingNecessaryInfoException)));
		}
        [Test]
        public void RegisterNew_RepoThrowsException_ThrowsRepoException()
        {
            A.CallTo (() => _repo.Register (A<PluginDTO>.Ignored)).Throws<RepositoryException>();
            var e = TestHelpers.TryGetException (() => mgr.RegisterNew (_plgn));
            Assert.That (e, Is.Not.Null);
            Assert.That (e, Is.InstanceOf (typeof (RepositoryException)));
        }

        [Test]
        public void UpdatePlugin_ValidPlugin_UpdatesRepo()
        {
            _plgn.PluginId = Guid.NewGuid ();
            var dto = new PluginDTO () {
                AssemblyLocation = VALID_STRING,
                FullName = VALID_STRING,
                Id = VALID_GUID,
                PluginName = VALID_STRING
            };

            A.CallTo (() => _dtoFac.Convert (_plgn)).Returns (dto);
            mgr.UpdatePlugin (_plgn);
            A.CallTo (() => _repo.Update (dto)).MustHaveHappened();
        }

        [Test]
        public void UpdatePlugin_NoPluginId_ThrowsInvalidIdException()
        {
            var e = TestHelpers.TryGetException (() => mgr.UpdatePlugin (_plgn));
            Assert.That (e, Is.Not.Null);
            Assert.That (e, Is.InstanceOf (typeof (InvalidIdException)));
            A.CallTo (() => _repo.Update (A<PluginDTO>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void UpdatePlugin_InvalidPlugin_FactoryThrowsException_Throws()
        {
            A.CallTo (() => _dtoFac.Convert (_plgn)).Throws<PluginIsMissingNecessaryInfoException> ();
            _plgn.PluginId = Guid.NewGuid ();
            var e = TestHelpers.TryGetException (() => mgr.UpdatePlugin (_plgn));
            Assert.That (e, Is.Not.Null);
            Assert.That (e, Is.InstanceOf (typeof (PluginIsMissingNecessaryInfoException)));
            A.CallTo (() => _repo.Update (A<PluginDTO>.Ignored)).MustNotHaveHappened();
        }
        [Test]
        public void UninstallPlugin_ValidId_DeletesPlugin()
        {
            mgr.UninstallPlugin (VALID_GUID);
            A.CallTo (() => _repo.Delete (VALID_GUID)).MustHaveHappened();
        }

        [Test]
        public void UninstallPlugin_EmptyGuid_ThrowsInvalidIdException()
        {
            var eGuid = Guid.Empty;
            var e = TestHelpers.TryGetException (() => mgr.UninstallPlugin (eGuid));
            Assert.That (e, Is.InstanceOf (typeof (InvalidIdException)));
        }
        [Test]
        public void UninstallPlugin_EncountersRepoException_ThrowsRepoException()
        {
            A.CallTo (() => _repo.Delete (VALID_GUID)).Throws<RepositoryException>();
            var e = TestHelpers.TryGetException (() => mgr.UninstallPlugin (VALID_GUID));
            Assert.That (e, Is.InstanceOf (typeof (RepositoryException)));
        }

        [Test]
        public void GetAll_ReturnsPluginsIEnumerable()
        {
            var dtos = Enumerable.Repeat (new PluginDTO (), 5).ToList();
            A.CallTo (() => _repo.GetAll()).Returns (dtos);
            var duPlugins = A.CollectionOfFake<DisplayUnitPlugin> (5);
            A.CallTo (() => _fac.LoadPluginsFromDtos (dtos)).Returns (duPlugins);

            var plugins = mgr.GetAll ();

            Assert.That (plugins, Is.EqualTo (duPlugins));
        }

        [Test]
        public void GetAll_RepoThrowsException_ThrowsRepoException()
        {
            A.CallTo (() => _repo.GetAll ()).Throws<Exception>();
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




	}
}

