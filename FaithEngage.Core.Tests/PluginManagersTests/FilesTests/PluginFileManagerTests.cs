using System;
using NUnit.Framework;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FakeItEasy;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Tests;

namespace FaithEngage.Core.PluginManagers.Files
{
	[TestFixture]
	public class PluginFileManagerTests
	{
		private IPluginFileInfoRepository _repo;
		private IConverterFactory<PluginFileInfo, PluginFileInfoDTO> _dtoFac;
		private IPluginFileInfoFactory _factory;
		private DirectoryInfo _tempFolder;
		private DirectoryInfo _pluginFolder;
		private PluginFileManager _mgr;
		[SetUp]
		public void init()
		{
			_repo = A.Fake<IPluginFileInfoRepository>();
			_dtoFac = A.Fake<IConverterFactory<PluginFileInfo, PluginFileInfoDTO>>();
			_factory = A.Fake<IPluginFileInfoFactory>();

			_tempFolder = new DirectoryInfo("TEMP");
			if (!_tempFolder.Exists) _tempFolder.Create();
			_pluginFolder = new DirectoryInfo("PLUGINS");
			if (!_pluginFolder.Exists) _pluginFolder.Create();
			A.CallTo(() => _factory.TempFolder).Returns(_tempFolder);
			A.CallTo(() => _factory.PluginsFolder).Returns(_pluginFolder);
			A.CallTo(() => _factory.GetBasePluginPath(A<Guid>.Ignored))
			 .ReturnsLazily(
				 (Guid p) => Path.Combine(_pluginFolder.FullName, p.ToString())
			   );
			_mgr = new PluginFileManager(_repo, _dtoFac, _factory);
		}

		[Test]
		public void DeleteAllFilesForPlugin_ValidPluginId()
		{
			var id = Guid.NewGuid();
			var subDir = _pluginFolder.CreateSubdirectory(id.ToString());
			Assert.That(subDir.Exists);
			_mgr.DeleteAllFilesForPlugin(id);
			Assert.That(!Directory.Exists(subDir.FullName));
			A.CallTo(() => _repo.DeleteAllFilesForPlugin(id)).MustHaveHappened();
		}

		[Test]
		public void DeleteAllFilesForPlugin_InvalidPluginId_FailsSilently()
		{
			var id = Guid.NewGuid();
			_mgr.DeleteAllFilesForPlugin(id);
			A.CallTo(() => _repo.DeleteAllFilesForPlugin(id)).MustHaveHappened();
		}

		[Test]
		public void DeleteAllFilesForPlugin_RepoThrowsException_FailsSilently()
		{
            A.CallTo(() => _repo.DeleteAllFilesForPlugin(A<Guid>.Ignored)).Throws<RepositoryException>();
			var id = Guid.NewGuid();
			var subDir = _pluginFolder.CreateSubdirectory(id.ToString());
			Assert.That(subDir.Exists);
			_mgr.DeleteAllFilesForPlugin(id);
		}

		[Test]
		public void DeleteFile_ValidFileId_ExistingFile()
		{
			var id = Guid.NewGuid();
			var plug = Guid.NewGuid();
			var plugFolder = _pluginFolder.CreateSubdirectory(plug.ToString());
			var path = Path.Combine(_pluginFolder.FullName, "TestFile.txt");
			File.WriteAllText(path, "Testing This file.");
			var dto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "TestFile.txt",
				PluginId = id,
				RelativePath = "TestFile.txt"
			};
			var file = new FileInfo(path);
			var pfile = new PluginFileInfo(id, file);

			A.CallTo(() => _repo.GetFileInfo(id)).Returns(dto);
			A.CallTo(() => _factory.Convert(dto)).Returns(pfile);

			_mgr.DeleteFile(id);

			Assert.That(!File.Exists(path));

			A.CallTo(() => _repo.DeleteFileRecord(id)).MustHaveHappened();
			plugFolder.Delete(true);
		}

		[Test]
		public void DeleteFile_NonExistingFile_FailsSilently()
		{
			var id = Guid.NewGuid();
			var plug = Guid.NewGuid();
			var plugFolder = _pluginFolder.CreateSubdirectory(plug.ToString());
			var path = Path.Combine(_pluginFolder.FullName, "TestFile.txt");
			var dto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "TestFile.txt",
				PluginId = id,
				RelativePath = "TestFile.txt"
			};
			var file = new FileInfo(path);
			var pfile = new PluginFileInfo(id, file);

			A.CallTo(() => _repo.GetFileInfo(id)).Returns(dto);
			A.CallTo(() => _factory.Convert(dto)).Returns(pfile);

			_mgr.DeleteFile(id);

			Assert.That(!File.Exists(path));

			A.CallTo(() => _repo.DeleteFileRecord(id)).MustHaveHappened();
			plugFolder.Delete(true);
		}

		[Test]
		public void DeleteFile_RepoThrowsException_FailsSilently()
		{
			var id = Guid.NewGuid();
			var plug = Guid.NewGuid();
			var plugFolder = _pluginFolder.CreateSubdirectory(plug.ToString());
			var path = Path.Combine(_pluginFolder.FullName, "TestFile.txt");
			File.WriteAllText(path, "Testing This file.");
			var dto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "TestFile.txt",
				PluginId = id,
				RelativePath = "TestFile.txt"
			};
			var file = new FileInfo(path);
			var pfile = new PluginFileInfo(id, file);

			A.CallTo(() => _repo.GetFileInfo(id)).Returns(dto);
			A.CallTo(() => _factory.Convert(dto)).Returns(pfile);
			A.CallTo(() => _repo.DeleteFileRecord(id)).Throws<RepositoryException>();
			_mgr.DeleteFile(id);

			Assert.That(!File.Exists(path));

			A.CallTo(() => _repo.DeleteFileRecord(id)).MustHaveHappened();
			plugFolder.Delete(true);
		}

		[Test]
		public void ExtractZipToTempFolder_ReturnsListOfFileInfo()
		{
			using (var zipFile = ZipFile.OpenRead(Path.Combine("TestingFiles", "pluginZip.zip")))
			{
				var id = Guid.NewGuid();
				var list = _mgr.ExtractZipToTempFolder(zipFile, id);
				Assert.That(list.Count, Is.EqualTo(4));
				Assert.That(list.All(p => p.Exists));
			}
			_tempFolder.EnumerateDirectories().ToList().ForEach(p => p.Delete(true));
		}

		[Test]
		public void FlushTempFolder_ValidKey_Flushes()
		{
			var id = Guid.NewGuid();
			var folder = _tempFolder.CreateSubdirectory(id.ToString());
			Assert.That(Directory.Exists(folder.FullName));
			_mgr.FlushTempFolder(id);
			Assert.That(!Directory.Exists(folder.FullName));
		}

		[Test]
		public void FlushTempFolder_InvalidKey_NoAction()
		{
			var id = Guid.NewGuid();
			Assert.That(!Directory.Exists(Path.Combine(_tempFolder.FullName, id.ToString())));
			_mgr.FlushTempFolder(id);
		}

		[Test]
		public void FlushTempFolder_NoKey_FoldersPresent_FlushesAllFolders()
		{
			_tempFolder.CreateSubdirectory("First");
			_tempFolder.CreateSubdirectory("Second");
			Assert.That(_tempFolder.EnumerateDirectories().Count() >= 2);
			_mgr.FlushTempFolder();
			Assert.That(_tempFolder.EnumerateDirectories().Count() == 0);
		}

		[Test]
		public void FlushTempFolder_NoFoldersPresent_NoAction()
		{
			if (_tempFolder.EnumerateDirectories().Count() > 0)
				_tempFolder.EnumerateDirectories().ToList().ForEach(p => p.Delete(true));

			_mgr.FlushTempFolder();
		}

        [Test]
        public void GetFile_ValidId_ObtainsFile()
        {
            var id = Guid.NewGuid ();
            var dto = new PluginFileInfoDTO ();
            dto.FileId = id;
            var pfile = new PluginFileInfo (Guid.NewGuid (), new FileInfo ("This is my file"));
            A.CallTo (() => _repo.GetFileInfo (id)).Returns (dto);
            A.CallTo (() => _factory.Convert (dto)).Returns (pfile);

            var file = _mgr.GetFile (id);

            Assert.That (file, Is.EqualTo (pfile));

        }

        [Test]
        public void GetFile_InvalidId_ReturnNull()
        {
            var id = Guid.NewGuid ();
            A.CallTo (() => _repo.GetFileInfo (id)).Returns (null);
            var file = _mgr.GetFile (id);

            Assert.That (file, Is.Null);
        }

        [Test]
        public void GetFile_RepoThrows_Throws()
        {
            var id = Guid.NewGuid ();
            var ex1 = new RepositoryException ("Test");
            A.CallTo (() => _repo.GetFileInfo (id)).Throws(ex1);
            var ex = TestHelpers.TryGetException(()=>  _mgr.GetFile (id));
            Assert.That (ex, Is.Not.Null);
            Assert.That (ex, Is.Not.EqualTo (ex1));
            Assert.That (ex, Is.InstanceOf<RepositoryException> ());
        }

        [Test]
        public void GetFile_FactoryThrows_Throws()
        {
            Assert.Inconclusive ();
        }

        [Test]
        public void GetFile_FactoryReturnsNull_ReturnsNull()
        {
            var id = Guid.NewGuid ();
            var dto = new PluginFileInfoDTO ();
            dto.FileId = id;
            A.CallTo (() => _repo.GetFileInfo (id)).Returns (dto);
            A.CallTo (() => _factory.Convert (dto)).Returns (null);

            var file = _mgr.GetFile (id);

            Assert.That (file, Is.Null);

        }
    }
}

