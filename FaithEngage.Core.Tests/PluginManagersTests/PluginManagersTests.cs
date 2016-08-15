﻿using System;
using NUnit.Framework;
using System.IO;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using FakeItEasy;
using System.Linq;
using FaithEngage.Core.PluginManagers.Files;

namespace FaithEngage.Core.PluginManagers
{
    [TestFixture]
    public class PluginManagersTests
    {
		private IPluginFileManager _fileMgr;
		private IPluginRepoManager _mgr;
		private PluginManager _pluginMgr;

		[SetUp]
		public void init()
		{
			_fileMgr = A.Fake<IPluginFileManager>();
			_mgr = A.Fake<IPluginRepoManager>();
			_pluginMgr = new PluginManager(_fileMgr, _mgr);
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

			A.CallTo(() => _fileMgr.GetFilesForPlugin(A<Guid>.Ignored)).Returns(pluginFiles);

			var num = _pluginMgr.Install(zipFile);

            zipFile.Dispose ();
			A.CallTo(() => _fileMgr.StoreFilesForPlugin(fileslist, key, true)).MustHaveHappened();
            A.CallTo (() => _fileMgr.FlushTempFolder (key)).MustHaveHappened();
			A.CallTo(() => _mgr.RegisterNew(A<Plugin>.Ignored)).MustHaveHappened();
            Assert.That (num == 1);
        }

        [Test]
        public void Install_ZipFileHasNoDlls_DoesNotInstall_ReturnsZero()
        {
            var zipFile = ZipFile.OpenRead (Path.Combine ("TestingFiles", "pluginZip.zip"));
            var zipDir = new DirectoryInfo (Path.Combine (Directory.GetCurrentDirectory (), "TestingFiles", "pluginZip"));
            Guid key = Guid.Empty;
            var files = zipDir.GetFiles ().Where(p=> p.Extension != ".dll");
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

            var num = _pluginMgr.Install (zipFile);

            zipFile.Dispose ();

            A.CallTo (() => _fileMgr.StoreFilesForPlugin (fileslist, key, true)).MustHaveHappened ();
            A.CallTo (() => _fileMgr.FlushTempFolder (key)).MustHaveHappened ();
            A.CallTo (() => _mgr.RegisterNew (A<Plugin>.Ignored)).MustNotHaveHappened();
            Assert.That(num == 0);
        }

        [Test]
        public void Uninstall_CallsUninstall()
        {
            var key = Guid.NewGuid ();
            _pluginMgr.Uninstall(key);

            A.CallTo (() => _mgr.UninstallPlugin (key)).MustHaveHappened();
        }
    }
}

