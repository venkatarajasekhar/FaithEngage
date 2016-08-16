using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using FaithEngage.Core.Factories;
using System.IO;
using FaithEngage.Core.PluginManagers.AssemblyReflector.Interfaces;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginManager : IPluginManager
	{
		private readonly IPluginFileManager _fileMgr;
        private readonly IPluginRepoManager _mgr;
        private readonly IAppFactory _factory;
        public PluginManager (IPluginFileManager fileManager, IPluginRepoManager mgr, IAppFactory factory)
		{
			_fileMgr = fileManager;
            _mgr = mgr;
            _factory = factory;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

		public int Install(ZipArchive zipFile)
		{
			var key = Guid.NewGuid();
            var files = _fileMgr.ExtractZipToTempFolder (zipFile, key);
            if(files.All(p=> p.Extension.ToLower() == ".zip")){
                int n = 0;
                foreach(var file in files){
                    using (var zip = new ZipArchive (file.Open (FileMode.Open))){
                        n += Install (zip);
                    };
                }
                return n;
            }

			var dlls = files.Where(p => p.Extension.ToLower() == ".dll").ToList();
            int num = 0;
            using (var reflector = _factory.GetOther<IAssemblyReflectionMgr> ()){
                foreach (var dll in dlls) {
                    var pluginTypes = getPluginTypes (dll, reflector);
                    foreach (var pluginType in pluginTypes) {
                        var ctor = pluginType.GetConstructors ().FirstOrDefault ();
                        if (ctor == null) continue;
                        var plugin = (Plugin)ctor.Invoke (new object [] { });
                        if (plugin == null) continue;
                        num++;
                        var plugid = _mgr.RegisterNew (plugin);
                        _fileMgr.StoreFilesForPlugin (files, plugid, true);
                        plugin.Install (_factory);
                    }
                }
            }
            _fileMgr.FlushTempFolder (key);
            return num;
		}

        private IEnumerable<Type> getPluginTypes(FileInfo dll, IAssemblyReflectionMgr reflector){
            var path = dll.FullName;
            var success = reflector.LoadAssembly (dll.FullName, dll.Name);
            IEnumerable<Type> types = null;
            try {
                types = reflector.Reflect (dll.FullName, a => a.GetTypes ());
            }catch(ReflectionTypeLoadException ex){
                types = ex.Types.Where (p => p != null);
            }
            var pluginTypes = types.Where (p => p.IsSubclassOf (typeof (Plugin)));
            return pluginTypes;
        }

        private Assembly CurrentDomain_AssemblyResolve (object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split (',') [0];
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

