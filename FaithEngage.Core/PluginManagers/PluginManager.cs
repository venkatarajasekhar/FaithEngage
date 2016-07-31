using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

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
			var dlls = pluginFiles.Where(p => p.Value.FileInfo.Extension.ToLower() == ".dll").ToList();
            int num = 0;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            foreach(var dll in dlls){
                var assembly = Assembly.LoadFile (dll.Value.FileInfo.FullName);
                IEnumerable<Type> types = null;
                try{
                    types = assembly.GetTypes ();
                }catch(ReflectionTypeLoadException ex){
                    types = ex.Types.Where(p => p != null);
                }
                var pluginTypes = types.Where (p => p.IsSubclassOf (typeof (Plugin)));
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

        Assembly CurrentDomain_AssemblyResolve (object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split (',') [0];
            var core = Assembly.GetExecutingAssembly ();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies ();
            var foundAssembly = assemblies.FirstOrDefault (p => p.FullName.Split (',') [0] == name);
            return foundAssembly;
        }

        public void Uninstall(Guid pluginId)
		{
            _mgr.UninstallPlugin (pluginId);
		}
	}
}

