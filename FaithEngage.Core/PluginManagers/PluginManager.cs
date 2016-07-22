using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.RepoInterfaces;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginManager : IPluginManager
	{
		private readonly IPluginFileManager _fileMgr;
		private readonly IPluginRepository _repo;
		public PluginManager(IPluginFileManager fileManager, IPluginRepository repo)
		{
			_fileMgr = fileManager;
			_repo = repo;
		}

		public Guid Install(ZipArchive zipFile)
		{
			var key = Guid.NewGuid();
			var files = _fileMgr.ExtractZipToTempFolder(zipFile, key);
			_fileMgr.StoreFilesForPlugin(files, key, true);
			var pluginFiles = _fileMgr.GetFilesForPlugin(key);
		}

		public void Uninstall(Guid pluginId)
		{
			throw new NotImplementedException();
		}
	}
}

