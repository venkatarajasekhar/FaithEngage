using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using System.Linq;
using System.Reflection;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginManager : IPluginManager
	{
		private readonly IPluginFileManager _fileMgr;
        private readonly IPluginRepoManager _mgr;
        public PluginManager(IPluginFileManager fileManager, IPluginRepoManager mgr)
		{
			_fileMgr = fileManager;
            _mgr = mgr;
		}

		public int Install(ZipArchive zipFile)
		{
			var key = Guid.NewGuid();
			var files = _fileMgr.ExtractZipToTempFolder(zipFile, key);
			_fileMgr.StoreFilesForPlugin(files, key, true);
			var pluginFiles = _fileMgr.GetFilesForPlugin(key);
            var dlls = pluginFiles.Where (p => p.Value.FileInfo.Extension.ToLower () == ".dll");
            int num = 0;
            foreach(var dll in dlls){
                var assembly = Assembly.LoadFrom (dll.Value.FileInfo.FullName);
                var pluginTypes = assembly.GetTypes ().Where (p => p.IsSubclassOf (typeof (Plugin)));
                foreach(var pluginType in pluginTypes){
                    var ctor = pluginType.GetConstructors ().FirstOrDefault ();
                    if (ctor == null) continue;
                    var plugin = (Plugin)ctor.Invoke (new object [] { });
                    if (plugin == null) continue;
                    num++;
                    _mgr.RegisterNew (plugin);
                }                      
            }
            return num;
		}

		public void Uninstall(Guid pluginId)
		{
            _mgr.UninstallPlugin (pluginId);
		}
	}
}

