using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using FaithEngage.Core.Factories;
using System.IO;
using Newtonsoft.Json.Linq;
using FaithEngage.Core.Exceptions;
using System.Security;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core.PluginManagers
{
    public class PluginManager : IPluginManager
	{
		private readonly IPluginFileManager _fileMgr;
        private readonly IPluginRepoManager _mgr;
        private readonly IAppFactory _factory;
		private readonly IRegistrationService _regService;
		public PluginManager(IPluginFileManager fileManager,
							  IPluginRepoManager mgr,
							  IAppFactory factory,
							  IRegistrationService regService
		                     )
		{
			_fileMgr = fileManager;
            _mgr = mgr;
            _factory = factory;
			_regService = regService;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += pluginAssembly_Resolve;
        }

		public int Install(ZipArchive zipFile)
		{
			var key = Guid.NewGuid();
            IList<FileInfo> files = null;
            try {
                files = _fileMgr.ExtractZipToTempFolder (zipFile, key);
            } catch (PluginFileException ex) {
                throw new PluginLoadException ("There was a problem extracting the zipfile to the temp folder.", ex);
            }

            var pluginsFile = files.FirstOrDefault (p => p.Name == "plugins.json");
            if (pluginsFile == null) throw new PluginLoadException ("There was no file called plugins.json in the zip folder.");
            PluginPackage pPackage = null;
            try {
                pPackage = getPluginPackage (pluginsFile);
            }catch (SecurityException ex) {
                throw new PluginLoadException ("There were not sufficient permissions to read the file: " + pluginsFile.Name, ex);
            }catch (FileNotFoundException ex){
                throw new PluginLoadException ("File doesn't exist: " + pluginsFile.Name, ex);
            }catch(UnauthorizedAccessException ex){
                throw new PluginLoadException($"File is read only. {pluginsFile.Name}", ex);
            }catch(DirectoryNotFoundException ex){
                throw new PluginLoadException ($"Path is invalid: {pluginsFile.FullName}", ex);
            }catch(IOException ex){
                throw new PluginLoadException ($"An IO Exception occurred reading the file: {pluginsFile.Name}", ex);
            }

            int num = 0;
            foreach (var pinfo in pPackage.Plugins){
                var plugId = storeFilesForPlugin (pinfo, files);
                var dll = getDll (pinfo, plugId);
                if (dll == null){
                    _fileMgr.FlushTempFolder (key);
                    _fileMgr.DeleteAllFilesForPlugin (plugId);
                    throw new PluginLoadException 
                    ("Dll not found for plugin: " + pinfo.PluginTypeName);
                }
                var plugin = getPlugin (pinfo, dll);
                num++;
                _mgr.RegisterNew (plugin, plugId);
                try {
                    plugin.Install (_factory);
                } catch (Exception ex) {
                    throw new PluginInstallException 
                    ($"There was a problem installing the plugin {plugin.PluginName}", ex);
                }

            }
            _fileMgr.FlushTempFolder (key);
            return num;
		}

        public void Install<TPlugin>(IList<FileInfo> files = null) where TPlugin : Plugin, new()
        {
            var plugin = new TPlugin ();
            Guid plugId;
            if (files != null) plugId = storeFilesForPlugin (files);
            else plugId = Guid.NewGuid ();
            try {
                _mgr.RegisterNew (plugin, plugId);
            } catch (RepositoryException ex) {
                throw new PluginLoadException 
                ($"There was a problem registering the {plugin.PluginName} plugin to the database.", ex);
            }
            try {
                plugin.Install (_factory);
            } catch (Exception ex) {
                throw new PluginInstallException 
                ("There was a problem installing the {plugin.PluginName} plugin", ex);
            }

        }

        private PluginPackage getPluginPackage (FileInfo file)
        {
            string json;
            using (var reader = file.OpenText ()) {
                json = reader.ReadToEnd ();
            }
            var jobject = JObject.Parse (json);
            return jobject.ToObject<PluginPackage> ();
        }

        private Guid storeFilesForPlugin(PluginPackage.pluginInfo pinfo,IList<FileInfo> allFiles)
        {
            var plugId = Guid.NewGuid ();
            var relPaths = pinfo.Files.Select (p => Path.Combine (p.Split ('/', '\\')));

            var pFiles = allFiles.Where (p => relPaths.Any (q => p.FullName.Contains (q)));
            try {
                _fileMgr.StoreFilesForPlugin (pFiles.ToList (), plugId, true);
            } catch (PluginFileException ex) {
                throw new PluginLoadException($"There was a problem storing files for {pinfo.PluginTypeName}.", ex);
            }
           
            return plugId;
        }

        private Guid storeFilesForPlugin(IList<FileInfo> files){
            var existentFiles = files.Where (p => {
                p.Refresh ();
                if (p.Exists) return true;
                return false;
            }).ToList ();
            var pluginId = Guid.NewGuid ();

            try {
                if (existentFiles.Count != 0)
                    _fileMgr.StoreFilesForPlugin (existentFiles, pluginId, true);
            } catch (PluginFileException ex) {
                throw new PluginLoadException ("There was a problem storing files.", ex);
            }
            return pluginId;
        }

        private FileInfo getDll(PluginPackage.pluginInfo pinfo, Guid plugId){
            var plugFiles = _fileMgr.GetFilesForPlugin (plugId);
            var dll = plugFiles.FirstOrDefault (
                p => p.Value.FileInfo.Name.ToLower ().Contains (
                    pinfo.DllName.ToLower ()
                ));
            return (dll.Value != null) ? dll.Value.FileInfo : null;
        }

        private Plugin getPlugin(PluginPackage.pluginInfo pinfo, FileInfo dll)
        {
            var assembly = Assembly.LoadFrom (dll.FullName);
            var types = assembly.GetTypes ();
            var ptype = types.FirstOrDefault (p => p.Name == pinfo.PluginTypeName);
            if (!ptype.IsSubclassOf (typeof (Plugin))) throw new PluginLoadException ("Plugin type is not derived from Plugin");
            var ctor = ptype.GetConstructors ().FirstOrDefault ();
            if (ctor == null) throw new PluginLoadException ("Plugin has no valid constructors");
            var plugin = (Plugin)ctor.Invoke (new object [] { });
            if (plugin == null) throw new PluginLoadException ("Plugin could not be constructed");
            return plugin;
        }



        private Assembly CurrentDomain_AssemblyResolve (object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split (',') [0];
            var assemblies = AppDomain.CurrentDomain.GetAssemblies ();
            var foundAssembly = assemblies.FirstOrDefault (p => p.FullName.Split (',') [0] == name);
            return foundAssembly;
        }

        private Assembly pluginAssembly_Resolve(object sender, ResolveEventArgs args)
        {
            var baseDir = Path.GetDirectoryName (args.RequestingAssembly.Location);
            var name = args.Name.Split (',') [0];
            var baseDirFiles = Directory.EnumerateFiles (baseDir, "*", SearchOption.AllDirectories).ToList ();
            var foundFile = baseDirFiles.FirstOrDefault (p => p.Contains (name + ".dll"));
            if (foundFile == null) return null;
            var assembly = Assembly.LoadFrom (foundFile);
            return assembly;
        }

        public void Uninstall(Guid pluginId)
		{
            Plugin plug;

			try
			{
				var success = _mgr.GetAllPlugins().TryGetValue(pluginId, out plug);
				if (!success) throw new PluginUninstallException($"Couldn't locate plugin with id {pluginId}");
				plug.Uninstall(_factory);
				_mgr.UninstallPlugin(pluginId);
			}
			catch (PluginUninstallException)
			{
				throw;
			}
			catch (RepositoryException ex)
			{
				throw new PluginUninstallException($"There was a problem registering the uninstallation with the db.", ex);
			}
			catch (Exception ex)
			{
				throw new PluginUninstallException($"There was a problem uninstalling the plugin.", ex); 
			}
		}

		public void InitializeAllPlugins()
		{
			var plugins = _mgr.GetAllPlugins();
			foreach (var plug in plugins)
			{
				try {
                    plug.Value.RegisterDependencies (_regService);
                } catch (Exception ex) {
                    throw new PluginDependencyRegistrationException ($"There was a problem registering dependencies on the plugin: {plug.Value.PluginName}.", ex);
                }
			}

			foreach (var plug in plugins)
			{
				try {
                    plug.Value.Initialize (_factory);
                } catch (Exception ex) {
                    throw new PluginInitializationException ($"There was a problem initializing the plugin: {plug.Value.PluginName}.", ex);
                }

			}
		}

        public bool CheckRegistered (Guid pluginId)
        {
            return _mgr.CheckRegistered (pluginId);
        }

        public bool CheckRegistered<TPlugin> () where TPlugin : Plugin
        {
            return _mgr.CheckRegistered<TPlugin> ();
        }
    }
}

