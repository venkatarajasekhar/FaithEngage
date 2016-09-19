using System;
using NUnit.Framework;
using System.IO;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using FakeItEasy;
using System.Linq;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.Factories;
using System.Collections.Generic;
using FaithEngage.Core.Tests;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core.PluginManagers
{
    [TestFixture]
    public class PluginManagersTests
    {
		private IPluginFileManager _fileMgr;
		private IPluginRepoManager _mgr;
		private PluginManager _pluginMgr;
        private IAppFactory _fac;
		private IRegistrationService _regService;

        private class dumbPlugin : Plugin
        {
            public override string PluginName {
                get {
                    return "Test Plugin";
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
                var config = FEFactory.ConfigManager;
                if (config.Get ("throw") == "yes") throw new Exception ();
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
		public void init()
		{
			_fileMgr = A.Fake<IPluginFileManager>();
			_mgr = A.Fake<IPluginRepoManager>();
            _fac = A.Fake<IAppFactory> ();
			_regService = A.Fake<IRegistrationService>();
			_pluginMgr = new PluginManager(_fileMgr, _mgr, _fac,_regService);
		}

        [Test]
        public void Install_ZipfileHasDlls_Installs_ReturnsNumber()
        {
            var zipFile = ZipFile.OpenRead (Path.Combine ("TestingFiles", "pluginZip.zip"));
			var zipDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),"TestingFiles", "pluginZip"));
			Guid key = Guid.Empty;
			var files = zipDir.GetFiles();
			var fileslist = files.ToList();
			A.CallTo(() => _fileMgr.ExtractZipToTempFolder(zipFile, A<Guid>.Ignored))
			 .ReturnsLazily((ZipArchive z, Guid k) =>
			 {
				 key = k;
				 return fileslist;
			});

			var fileId = Guid.NewGuid();
			var pluginFiles = files.ToDictionary(p => 
			{ 
				fileId = Guid.NewGuid();
				return fileId; 
			}, p =>
			{
			  var pfile = new PluginFileInfo(key, p);
			  pfile.FileId = Guid.NewGuid();
			  return pfile;
			});
            A.CallTo (() => _fileMgr.GetFilesForPlugin (A<Guid>.Ignored)).Returns (pluginFiles);

			var num = _pluginMgr.Install(zipFile);

            zipFile.Dispose ();
            A.CallTo(() => _fileMgr.StoreFilesForPlugin(A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).MustHaveHappened();
            A.CallTo (() => _fileMgr.FlushTempFolder (key)).MustHaveHappened();
            A.CallTo(() => _mgr.RegisterNew(A<Plugin>.Ignored, A<Guid>.Ignored)).MustHaveHappened();
            Assert.That (num == 2);
        }

        [Test]
        public void Install_ZipFileHasNoDlls_DoesNotInstall_ThrowsPluginLoadException()
        {
            var zipFile = ZipFile.OpenRead (Path.Combine ("TestingFiles", "pluginZip.zip"));
            var zipDir = new DirectoryInfo (Path.Combine (Directory.GetCurrentDirectory (), "TestingFiles", "pluginZip"));
            Guid key = Guid.Empty;
            var files = zipDir.GetFiles ().Where (p => p.Extension != ".dll");
            var fileslist = files.ToList ();
            A.CallTo (() => _fileMgr.ExtractZipToTempFolder (zipFile, A<Guid>.Ignored))
             .ReturnsLazily ((ZipArchive z, Guid k) => {
                 key = k;
                 return fileslist;
             });

            var fileId = Guid.NewGuid ();
            var pluginFiles = files.ToDictionary (p => {
                fileId = Guid.NewGuid ();
                return fileId;
            }, p => {
                var pfile = new PluginFileInfo (key, p);
                pfile.FileId = Guid.NewGuid ();
                return pfile;
            });

            A.CallTo (() => _fileMgr.GetFilesForPlugin (A<Guid>.Ignored)).Returns (pluginFiles);

            var e = TestHelpers.TryGetException(()=> _pluginMgr.Install (zipFile));

            zipFile.Dispose ();

            A.CallTo (() => _fileMgr.StoreFilesForPlugin (A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).MustHaveHappened ();
            A.CallTo (() => _fileMgr.FlushTempFolder (key)).MustHaveHappened ();
            A.CallTo (() => _mgr.RegisterNew (A<Plugin>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            Assert.That (e, Is.InstanceOf<PluginLoadException> ());
        }

        [Test]
        public void InstallGeneric_HasFiles_StoresRegistersAndInstalls()
        {
            var filesDir = new DirectoryInfo ("TestingFiles");
            var files = filesDir.EnumerateFiles ("*.jpg", SearchOption.AllDirectories).ToList ();

            Assert.That (files.All (p => p.Exists));

            _pluginMgr.Install<dumbPlugin> (files);

            A.CallTo (() => _fileMgr.StoreFilesForPlugin (A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).MustHaveHappened();
            A.CallTo (() => _mgr.RegisterNew (A<dumbPlugin>.Ignored, A<Guid>.Ignored)).MustHaveHappened();
            A.CallTo (() => _fac.ConfigManager).MustHaveHappened ();
        }

        [Test]
        public void InstallGeneric_HasFilesButAreNonExistant_StoresRegistersAndInstalls ()
        {
            var files = new List<FileInfo> { new FileInfo ("TestText.txt")};

            Assert.That (files.All (p => !p.Exists));

            _pluginMgr.Install<dumbPlugin> (files);

            A.CallTo (() => _fileMgr.StoreFilesForPlugin (A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).MustNotHaveHappened ();
            A.CallTo (() => _mgr.RegisterNew (A<dumbPlugin>.Ignored, A<Guid>.Ignored)).MustHaveHappened ();
            A.CallTo (() => _fac.ConfigManager).MustHaveHappened ();
        }

        [Test]
        public void InstallGeneric_HasNoFiles_RegistersAndInstalls ()
        {
            _pluginMgr.Install<dumbPlugin> ();

            A.CallTo (() => _fileMgr.StoreFilesForPlugin (A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).MustNotHaveHappened ();
            A.CallTo (() => _mgr.RegisterNew (A<dumbPlugin>.Ignored, A<Guid>.Ignored)).MustHaveHappened ();
            A.CallTo (() => _fac.ConfigManager).MustHaveHappened ();
        }

        [Test]
        public void InstallGeneric_HasFiles_FileSystemException_ThrowsPluginLoadException ()
        {
            var filesDir = new DirectoryInfo ("TestingFiles");
            var files = filesDir.EnumerateFiles ("*.jpg", SearchOption.AllDirectories).ToList ();

            Assert.That (files.All (p => p.Exists));

            A.CallTo (() => _fileMgr.StoreFilesForPlugin (A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).Throws<PluginFileException> ();
            var e = TestHelpers.TryGetException(()=> _pluginMgr.Install<dumbPlugin> (files));

            Assert.That (e, Is.InstanceOf<PluginLoadException>());
            Assert.That (e.InnerException, Is.InstanceOf<PluginFileException> ());
            A.CallTo (() => _fileMgr.StoreFilesForPlugin (A<List<FileInfo>>.Ignored, A<Guid>.Ignored, true)).MustHaveHappened ();
        }

        [Test]
        public void InstallGeneric_RepoException_ThrowsPluginLoadException ()
        {
            A.CallTo (() => _mgr.RegisterNew (A<dumbPlugin>.Ignored, A<Guid>.Ignored)).Throws<RepositoryException> ();
            var e = TestHelpers.TryGetException (() => _pluginMgr.Install<dumbPlugin> ());

            Assert.That (e, Is.InstanceOf<PluginLoadException> ());
            Assert.That (e.InnerException, Is.InstanceOf<RepositoryException> ());
            A.CallTo (() => _mgr.RegisterNew (A<dumbPlugin>.Ignored, A<Guid>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void InstallGeneric_ExceptionFromPluginInstall_ThrowsPluginInstallException ()
        {
            A.CallTo (() => _fac.ConfigManager.Get ("throw")).Returns ("yes");
            var e = TestHelpers.TryGetException (() => _pluginMgr.Install<dumbPlugin> ());

            Assert.That (e, Is.InstanceOf<PluginInstallException> ());
            Assert.That (e.InnerException, Is.InstanceOf<Exception> ());
            A.CallTo (() => _mgr.RegisterNew (A<dumbPlugin>.Ignored, A<Guid>.Ignored)).MustHaveHappened ();
        }

        [Test]
        public void Uninstall_CallsUninstall()
        {
            Assert.Inconclusive ("Needs additional testing.");
            var key = Guid.NewGuid ();
            _pluginMgr.Uninstall(key);

            A.CallTo (() => _mgr.UninstallPlugin (key)).MustHaveHappened();
        }

		[Test]
		public void InitializeAllPlugins_RegistersAndInitializes()
		{
            Assert.Inconclusive("No test yet");
		}

        [Test]
        public void CheckRegistered_RegisteredId_ReturnsTrue()
        {
            Assert.Inconclusive ("No test yet");
        }

        [Test]
        public void CheckRegistered_UnRegisteredId_ReturnsFalse ()
        {
            Assert.Inconclusive ("No test yet");
        }

        [Test]
        public void CheckRegistered_RegisteredType_ReturnsTrue ()
        {
            Assert.Inconclusive ("No test yet");
        }

        [Test]
        public void CheckRegistered_UnRegisteredType_ReturnsFalse ()
        {
            Assert.Inconclusive ("No test yet");
        }
    }
}

