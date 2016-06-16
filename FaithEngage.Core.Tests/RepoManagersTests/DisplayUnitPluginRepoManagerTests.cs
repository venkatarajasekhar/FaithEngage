using System;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.RepoManagers;
using FakeItEasy;
using NUnit.Framework;
namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	[TestFixture]
	public class DisplayUnitPluginRepoManagerTests
	{
		private IDisplayUnitPluginRepository _repo;
		private IDisplayUnitPluginFactory _fac;
		private IDisplayUnitPluginDtoFactory _dtoFac;
		private const string VALID_STRING = "VALID";
		private const string INVALID_STRING = "INVALID";
		private Guid VALID_GUID = Guid.NewGuid();
		private DisplayUnitPlugin _plgn;

		[SetUp]
		public void Init()
		{
			_repo = A.Fake<IDisplayUnitPluginRepository>();
			_fac = A.Fake<IDisplayUnitPluginFactory>();
			_dtoFac = A.Fake<IDisplayUnitPluginDtoFactory>();
			_plgn = A.Fake<DisplayUnitPlugin>();
		}

		[Test]
		public void RegisterNew_ValidPlugin_RegistersAndReturnsId()
		{
			var dto = new DisplayUnitPluginDTO()
			{
				AssemblyLocation = VALID_STRING,
				FullName = VALID_STRING,
				Id = VALID_GUID,
				PluginName = VALID_STRING
			};
			A.CallTo(() => _dtoFac.ConvertFromPlugin(_plgn)).Returns(dto);
			A.CallTo(() => _repo.Register(A<DisplayUnitPluginDTO>.Ignored)).Returns(VALID_GUID);

			var mgr = new DisplayUnitPluginRepoManager(_repo,_fac,_dtoFac);
			var id = mgr.RegisterNew(_plgn);

			Assert.That(id, Is.EqualTo(VALID_GUID));
			Assert.That(_plgn.PluginId.HasValue);
		}

		[Test]
		[ExpectedException(typeof(PluginIsMissingNecessaryInfoException))]
		public void RegisterNew_InvalidPlugin_Throws()
		{
			A.CallTo(() => _dtoFac.ConvertFromPlugin(_plgn)).Throws<PluginIsMissingNecessaryInfoException>();
			var mgr = new DisplayUnitPluginRepoManager(_repo, _fac, _dtoFac);
			var id = mgr.RegisterNew(_plgn);
		}
	}
}

