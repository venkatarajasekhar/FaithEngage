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
using System.Collections.Generic;

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
				Assert.That(list.Count, Is.EqualTo(5));
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

		[Test]
		public void GetFilesForPlugin_ValidId_ReturnsLoadedDict()
		{
			var pluginId = Guid.NewGuid();
			var dtos = Enumerable
				.Repeat("x", 5)
	            .Select(
	                (arg1, arg2) => new PluginFileInfoDTO() 
						{ 
							Name = "Test", 
							FileId = Guid.NewGuid(), 
							PluginId = pluginId
						}
                )
				.ToList();

			A.CallTo(() => _repo.GetAllFilesForPlugin(pluginId)).Returns(dtos);
			A.CallTo(
				() => _factory.Convert(A<PluginFileInfoDTO>.Ignored))
			 	.ReturnsLazily(
				 	(PluginFileInfoDTO d) => 
			 		new PluginFileInfo(pluginId, new FileInfo("testing"),d.FileId)
				);
			var files = _mgr.GetFilesForPlugin(pluginId);

			Assert.That(files.All(p=> dtos.Any(q=> q.FileId == p.Value.FileId)));
		}

		[Test]
		public void GetFilesForPlugin_InvalidId_ReturnsNull()
		{
			var pluginId = Guid.NewGuid();
			A.CallTo(() => _repo.GetAllFilesForPlugin(pluginId)).Returns(null);

			var files = _mgr.GetFilesForPlugin(pluginId);

			Assert.That(files, Is.Null);

		}
		[Test]
		public void GetFilesForPlugin_ValidId_NoFiles_ReturnsEmptyDict()
		{
			var pluginId = Guid.NewGuid();
			A.CallTo(() => _repo.GetAllFilesForPlugin(pluginId)).Returns(new List<PluginFileInfoDTO>());
			var files = _mgr.GetFilesForPlugin(pluginId);

			Assert.That(files, Is.Not.Null);
			Assert.That(files.Count == 0);
		}

		[Test]
		public void GetFilesForPlugin_RepoThrows_ThrowsNewRepoException()
		{
			var pluginId = Guid.NewGuid();
			A.CallTo(() => _repo.GetAllFilesForPlugin(pluginId)).Throws(new RepositoryException("Test"));
			var e = TestHelpers.TryGetException(()=>_mgr.GetFilesForPlugin(pluginId));

			Assert.That(e, Is.InstanceOf<RepositoryException>());
			Assert.That(e.Message != "Test");
		}

		[Test]
		public void RenameFile_ValidId_ValidPath_ExistingFile_RenamesFile()
		{
			var id = Guid.NewGuid();
			var plug = Guid.NewGuid();
			var plugFolder = _pluginFolder.CreateSubdirectory(plug.ToString());
			var path = Path.Combine(_pluginFolder.FullName, plug.ToString(), "TestFile.txt");
			var newPath = Path.Combine(_pluginFolder.FullName, plug.ToString(), "testing", "testfile1.txt");
			File.WriteAllText(path, "Testing This file.");
			var dto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "TestFile.txt",
				PluginId = id,
				RelativePath = "TestFile.txt"
			};
			var file = new FileInfo(path);
			var pfile = new PluginFileInfo(plug, file);
			var newDto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "testfile1.txt",
				PluginId = plug,
				RelativePath = "testing/testfile1.txt"
			};

			A.CallTo(() => _repo.GetFileInfo(id)).Returns(dto);
			A.CallTo(() => _factory.Convert(dto)).Returns(pfile);
			A.CallTo(() => _factory.GetRenamedPath(pfile, "testing/testfile1.txt")).Returns(newPath);
			A.CallTo(() => _dtoFac.Convert(pfile)).Returns(newDto);

			Assert.That(File.Exists(path));
			_mgr.RenameFile(id, "testing/testfile1.txt");

			A.CallTo(() => _repo.UpdateFile(newDto)).MustHaveHappened();
			Assert.That(File.Exists(newPath));
			Assert.That(!File.Exists(path));

			plugFolder.Delete(true);
		}

		[Test]
		public void RenameFile_FactoryReturnsNull_ThrowsInvalidFileException()
		{
			var id = Guid.NewGuid();
			var newRelPath = "testing/testfile1.txt";
			A.CallTo(() => _factory.Convert(A<PluginFileInfoDTO>.Ignored)).Returns(null);
			var e = TestHelpers.TryGetException(() => _mgr.RenameFile(id, newRelPath));

			Assert.That(e, Is.InstanceOf<InvalidFileException>());
		}

		[Test]
		public void RenameFile_RepoThrows_ThrowsNew()
		{
			var id = Guid.NewGuid();
			var plug = Guid.NewGuid();
			var plugFolder = _pluginFolder.CreateSubdirectory(plug.ToString());
			var path = Path.Combine(_pluginFolder.FullName, plug.ToString(), "TestFile.txt");
			var newPath = Path.Combine(_pluginFolder.FullName, plug.ToString(), "testing", "testfile1.txt");
			File.WriteAllText(path, "Testing This file.");
			var dto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "TestFile.txt",
				PluginId = id,
				RelativePath = "TestFile.txt"
			};
			var file = new FileInfo(path);
			var pfile = new PluginFileInfo(plug, file);
			var newDto = new PluginFileInfoDTO()
			{
				FileId = id,
				Name = "testfile1.txt",
				PluginId = plug,
				RelativePath = "testing/testfile1.txt"
			};

			var repoE = new RepositoryException();

			A.CallTo(() => _repo.GetFileInfo(id)).Returns(dto);
			A.CallTo(() => _factory.Convert(dto)).Returns(pfile);
			A.CallTo(() => _factory.GetRenamedPath(pfile, "testing/testfile1.txt")).Returns(newPath);
			A.CallTo(() => _dtoFac.Convert(pfile)).Returns(newDto);
			A.CallTo(() => _repo.UpdateFile(newDto)).Throws(repoE);
			Assert.That(File.Exists(path));

			var e = TestHelpers.TryGetException(()=> _mgr.RenameFile(id, "testing/testfile1.txt"));

			A.CallTo(() => _repo.UpdateFile(newDto)).MustHaveHappened();
			Assert.That(File.Exists(newPath));
			Assert.That(!File.Exists(path));
			Assert.That(e, Is.InstanceOf<RepositoryException>());
			Assert.That(e, Is.Not.EqualTo(repoE));

			plugFolder.Delete(true);
		}

		[Test]
		public void StoreFilesForPlugin_ValidFiles_PluginNotPrexisting_Stores()
		{
			var currentDir = Directory.GetCurrentDirectory();
			var testFilesDir = new DirectoryInfo(Path.Combine(currentDir, "TestingFiles"));
			var files = testFilesDir.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
			var plugid = Guid.NewGuid();

            A.CallTo (() => _factory.Create (A<FileInfo>.Ignored, plugid))
             .ReturnsLazily ((FileInfo f, Guid g) => new PluginFileInfo (g, f));
            A.CallTo (() => _dtoFac.Convert (A<PluginFileInfo>.Ignored))
             .ReturnsLazily ((PluginFileInfo p) =>
                             new PluginFileInfoDTO ());

            _mgr.StoreFilesForPlugin(files, plugid, true);

            A.CallTo (() => _repo.SaveFileInfo (A<PluginFileInfoDTO>.Ignored))
             .MustHaveHappened (Repeated.Exactly.Times(files.Count));
            var plugDirPath = Path.Combine (_pluginFolder.FullName, plugid.ToString ());
            var plugDir = new DirectoryInfo (plugDirPath);
            var plugFileNames = plugDir.EnumerateFiles ("*", SearchOption.AllDirectories).Select (p => p.Name);
            var copiedfileNames = files.Select (p => p.Name);
            Assert.That (plugFileNames.All (p => copiedfileNames.Any (c => p == c)));

            plugDir.Delete (true);
		}

        [Test]
        public void StoreFilesForPlugin_ValidFiles_PluginPreExisting_Stores(){
            var currentDir = Directory.GetCurrentDirectory ();
            var testFilesDir = new DirectoryInfo (Path.Combine (currentDir, "TestingFiles"));
            var files = testFilesDir.EnumerateFiles ("*", SearchOption.AllDirectories).ToList ();
            var plugid = Guid.NewGuid ();
            var plugDirPath = Path.Combine (_pluginFolder.FullName, plugid.ToString ());
            var plugDir = new DirectoryInfo (plugDirPath);
            plugDir.Create ();


            A.CallTo (() => _factory.Create (A<FileInfo>.Ignored, plugid))
             .ReturnsLazily ((FileInfo f, Guid g) => new PluginFileInfo (g, f));
            A.CallTo (() => _dtoFac.Convert (A<PluginFileInfo>.Ignored))
             .ReturnsLazily ((PluginFileInfo p) =>
                             new PluginFileInfoDTO ());

            _mgr.StoreFilesForPlugin (files, plugid, true);

            A.CallTo (() => _repo.SaveFileInfo (A<PluginFileInfoDTO>.Ignored))
             .MustHaveHappened (Repeated.Exactly.Times (files.Count));
            
            var plugFileNames = plugDir.EnumerateFiles ("*", SearchOption.AllDirectories).Select (p => p.Name);
            var copiedfileNames = files.Select (p => p.Name);
            Assert.That (plugFileNames.All (p => copiedfileNames.Any (c => p == c)));

            plugDir.Delete (true);
        }

        [Test]
        public void StoreFilesForPlugin_NonExistentFiles_NoAction(){
            var file1 = new FileInfo ("Test1.text");
            var file2 = new FileInfo ("Test2.text");
            var file3 = new FileInfo ("Test3.text");

            var files = new List<FileInfo> () { file1, file2, file3};
            var plugid = Guid.NewGuid ();

            _mgr.StoreFilesForPlugin (files, plugid, true);

            A.CallTo (() => _repo.SaveFileInfo (A<PluginFileInfoDTO>.Ignored)).MustNotHaveHappened();
        }

    }
}

