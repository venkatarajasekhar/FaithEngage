using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Config;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Facade;
using FakeItEasy;
using NUnit.Framework;
using System.Linq;
namespace FaithEngage.IntegrationTests
{
	public class FaithEngagePluginsTests
	{
		
		private IContainer _container;
		private IBootList _bootlist;
		private IPluginManager _mgr;



		public class config : IConfigManager
		{
			public string this[string key]
			{
				get
				{
					return GetValue(key);
				}

				set
				{
					SetValue(key, value);
				}
			}

			public string PluginsFolderPath { get { return "PLUGINS"; } set { return; } }

			public string TempFolderPath { get { return "TEMP"; } set { return; } }

			public string GetValue(string key)
			{
				throw new NotImplementedException();
			}

			public string SetValue(string Key, string value)
			{
				throw new NotImplementedException();
			}
		}
		public class repo : IPluginRepository
		{
			private Dictionary<Guid, PluginDTO> dtoRepo = new Dictionary<Guid, PluginDTO>(); 

			public void Delete(Guid pluginId)
			{
				throw new NotImplementedException();
			}

			public List<PluginDTO> GetAll()
			{
				return dtoRepo.Values.ToList();
			}

			public List<PluginDTO> GetAll(PluginTypeEnum pluginType)
			{
				return dtoRepo.Values.Where(p => p.PluginType == pluginType).ToList();
			}

			public PluginDTO GetById(Guid pluginId)
			{
				return dtoRepo[pluginId];
			}

			public void Register(PluginDTO plugin, Guid pluginId)
			{
				plugin.Id = pluginId;
				dtoRepo.Add(pluginId, plugin);
			}

			public void Update(PluginDTO plugin)
			{
				throw new NotImplementedException();
			}
		}
		public class fileRepo : IPluginFileInfoRepository
		{
			private List<PluginFileInfoDTO> _dtos = new List<PluginFileInfoDTO>();
			public void DeleteAllFilesForPlugin(Guid pluginId)
			{
				throw new NotImplementedException();
			}

			public void DeleteFileRecord(Guid fileId)
			{
				throw new NotImplementedException();
			}

			public IList<PluginFileInfoDTO> GetAllFilesForPlugin(string PluginName)
			{
				throw new NotImplementedException();
			}

			public IList<PluginFileInfoDTO> GetAllFilesForPlugin(Guid pluginId)
			{
				return _dtos;
			}

			public PluginFileInfoDTO GetFileInfo(Guid fileId)
			{
				throw new NotImplementedException();
			}

			public void SaveFileInfo(PluginFileInfoDTO dto)
			{
				_dtos.Add(dto);
			}

			public void UpdateFile(PluginFileInfoDTO dto)
			{
				throw new NotImplementedException();
			}
		}



		[SetUp]
		public void Init()
		{
			var initializer = new Initializer();
			_bootlist = initializer.LoadedBootList;
			_container = initializer.Container;
			_container.Register<IConfigManager, config>();
			_container.Register<IPluginRepository, repo>();
			_container.Register<IPluginFileInfoRepository, fileRepo>();
			_bootlist.Load<PluginBootstrapper>();
			Console.Write(_bootlist.RegisterAllDependencies(true));

		}





		[Test]
		public void InstallFaithEngagePlugins_StubbedRepoAndConfig()
		{
			var mgr = _container.Resolve<IPluginManager>();
			int numInstalled;
			using (var zipFile = ZipFile.OpenRead(Path.Combine("TestingFiles", "FaithEngage.Plugins.zip")))
			{
				numInstalled = mgr.Install(zipFile);
			}

			var factory = _container.Resolve<IAppFactory>();
			var pluginContainer = factory.GetOther<IDisplayUnitPluginContainer>();
			var repoManager = factory.DisplayUnitsPluginRepo;
			var regService = factory.RegistrationService;
            var e = TestHelpers.TryGetException (() => mgr.InitializeAllPlugins ());
            Assert.That (e, Is.Null);
			foreach (var plugin in repoManager.GetAll())
			{
				pluginContainer.Register(plugin);
			}
		}
	}
}

