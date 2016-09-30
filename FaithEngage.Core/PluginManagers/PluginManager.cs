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
            /* Because we're dealing with loading assemblies, we need to make sure that any unresolved
             * assembly dependencies can be resolved. */
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += pluginAssembly_Resolve;
        }
		/// <summary>
		/// Installs a plugin from a zip archive.
		/// </summary>
		/// <param name="zipFile">Zip file.</param>
		public int Install(ZipArchive zipFile)
		{
			var key = Guid.NewGuid();
            IList<FileInfo> files = null;
            //1. Extract and obtain references to all the files in the ziparchive.
			try {
                files = _fileMgr.ExtractZipToTempFolder (zipFile, key);
            } catch (PluginFileException ex) {
                throw new PluginLoadException ("There was a problem extracting the zipfile to the temp folder.", ex);
            }
			//2. Get the file called "plugins.json"
            var pluginsFile = files.FirstOrDefault (p => p.Name == "plugins.json");
            if (pluginsFile == null) throw new PluginLoadException ("There was no file called plugins.json in the zip folder.");
            //3. Obtain the pluginPackage from the plugins.json file.
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
			//4. Loop through the plugins in the package (there could be more than one). For each one...
            foreach (var pinfo in pPackage.Plugins){
                //4a. Store the files for the plugin as needed by the plugins.json file.
				var plugId = storeFilesForPlugin (pinfo, files);
                //4b. Get the main assembly for the plugin.
				var dll = getDll (pinfo, plugId);
                if (dll == null){//If the dll can't be obtained, fail and and throw.
                    _fileMgr.FlushTempFolder (key);
                    _fileMgr.DeleteAllFilesForPlugin (plugId);
                    throw new PluginLoadException 
                    ("Dll not found for plugin: " + pinfo.PluginTypeName);
                }
                //4c. Get the plugin from the assembly.
				var plugin = getPlugin (pinfo, dll);
                num++;
                //4d. Register the plugin.
				_mgr.RegisterNew (plugin, plugId);
                //4e. Install the pugin.
				try {
                    plugin.Install (_factory);
                } catch (Exception ex) {
                    throw new PluginInstallException 
                    ($"There was a problem installing the plugin {plugin.PluginName}", ex);
                }

            }
            //5. clear out the temp folder.
			_fileMgr.FlushTempFolder (key);
            //Return the number of Plugins installed.
			return num;
		}

		/// <summary>
		/// Installs the specified Plugin type with the given files.
		/// </summary>
		/// <param name="files">Files.</param>
		/// <typeparam name="TPlugin">The 1st type parameter.</typeparam>
        public void Install<TPlugin>(IList<FileInfo> files = null) where TPlugin : Plugin, new()
        {
            //1. Insantiate the plugin.
			var plugin = new TPlugin ();
            Guid plugId;
            //2. Store the files for the plugin and get a plugin id.
			if (files != null) plugId = storeFilesForPlugin (files);
            else plugId = Guid.NewGuid ();
            //3. Register the new plugin.
			try {
                _mgr.RegisterNew (plugin, plugId);
            } catch (RepositoryException ex) {
                throw new PluginLoadException 
                ($"There was a problem registering the {plugin.PluginName} plugin to the database.", ex);
            }
            //4. Install the plugin.
			try {
                plugin.Install (_factory);
            } catch (Exception ex) {
                throw new PluginInstallException 
                ("There was a problem installing the {plugin.PluginName} plugin", ex);
            }

        }

		/// <summary>
		/// Gets the plugin package from the fileinfo for the plugins.json file.
		/// </summary>
		/// <returns>The plugin package.</returns>
		/// <param name="file">File.</param>
        private PluginPackage getPluginPackage (FileInfo file)
        {
            string json;
            using (var reader = file.OpenText ()) {
                json = reader.ReadToEnd ();
            }
            var jobject = JObject.Parse (json);
            return jobject.ToObject<PluginPackage> ();
        }
		/// <summary>
		/// Stores the files for a new plugin and returns the new id for those files. Used for plugins loaded from a 
		/// zip archive.
		/// </summary>
		/// <returns>The files for plugin.</returns>
		/// <param name="pinfo">Pinfo.</param>
		/// <param name="allFiles">All files.</param>
        private Guid storeFilesForPlugin(PluginPackage.pluginInfo pinfo,IList<FileInfo> allFiles)
        {
            var plugId = Guid.NewGuid ();
            //Get the OS safe paths of each of the needed files for the plugin.
			var relPaths = pinfo.Files.Select (p => Path.Combine (p.Split ('/', '\\')));
			//Get the files where the relative paths in the pinfo are found in the allFiles list. 
            var pFiles = allFiles.Where (p => relPaths.Any (q => p.FullName.Contains (q)));
            //Store those files.
			try {
                _fileMgr.StoreFilesForPlugin (pFiles.ToList (), plugId, true);
            } catch (PluginFileException ex) {
                throw new PluginLoadException($"There was a problem storing files for {pinfo.PluginTypeName}.", ex);
            }
            return plugId;
        }
		/// <summary>
		/// Stores the files for plugin and returns the new id for those files. Used for plugins loaded from a passed 
		/// in plugin type.
		/// </summary>
		/// <returns>The files for plugin.</returns>
		/// <param name="files">Files.</param>
        private Guid storeFilesForPlugin(IList<FileInfo> files){
            //Gets all the files that actually exist.
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
		/// <summary>
		/// Obtains the dll referenced in a plugininfo object from stored filesystem.
		/// </summary>
		/// <returns>The dll.</returns>
		/// <param name="pinfo">Pinfo.</param>
		/// <param name="plugId">Plug identifier.</param>
        private FileInfo getDll(PluginPackage.pluginInfo pinfo, Guid plugId){
            var plugFiles = _fileMgr.GetFilesForPlugin (plugId);
            var dll = plugFiles.FirstOrDefault (
                p => p.Value.FileInfo.Name.ToLower ().Contains (
                    pinfo.DllName.ToLower ()
                ));
            return (dll.Value != null) ? dll.Value.FileInfo : null;
        }
		/// <summary>
		/// Obtains the plugin specified in the plugininfo object from the passed in dll file.
		/// </summary>
		/// <returns>The plugin.</returns>
		/// <param name="pinfo">Pinfo.</param>
		/// <param name="dll">Dll.</param>
        private Plugin getPlugin(PluginPackage.pluginInfo pinfo, FileInfo dll)
        {
            //Load the assembly
			var assembly = Assembly.LoadFrom (dll.FullName);
            //Get all the types in the Assembly.
			var types = assembly.GetTypes ();
            //Look for the type with the name specified in the plugininfo object.
			var ptype = types.FirstOrDefault (p => p.Name == pinfo.PluginTypeName);
            //If the type isn't derived from Plugin, throw.
			if (!ptype.IsSubclassOf (typeof (Plugin))) throw new PluginLoadException ("Plugin type is not derived from Plugin");
            //Get the first construct for the plugin.
			var ctor = ptype.GetConstructors ().FirstOrDefault ();
            if (ctor == null) throw new PluginLoadException ("Plugin has no valid constructors");
            //Invoke the constructor to create the plugin.
			var plugin = (Plugin)ctor.Invoke (new object [] { });
            if (plugin == null) throw new PluginLoadException ("Plugin could not be constructed");
            return plugin;
        }


		/// <summary>
		/// Resolves the assembly from within the current domain.
		/// </summary>
		/// <returns>The domain assembly resolve.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
        private Assembly CurrentDomain_AssemblyResolve (object sender, ResolveEventArgs args)
        {
            //Get the assembly name, without the version number, etc...
			var name = args.Name.Split (',') [0];
			//Get all assemblies currently loaded in the current appdomain.
            var assemblies = AppDomain.CurrentDomain.GetAssemblies ();
            //Compare the searched for name with the names of all the assemblies.
			var foundAssembly = assemblies.FirstOrDefault (p => p.FullName.Split (',') [0] == name);
            return foundAssembly;
        }
		/// <summary>
		/// Resolves the assembly from within the other files surrounding the requesting assembly.
		/// </summary>
		/// <returns>The assembly resolve.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
        private Assembly pluginAssembly_Resolve(object sender, ResolveEventArgs args)
        {
            //Get the directory of the requesting assembly.
			var baseDir = Path.GetDirectoryName (args.RequestingAssembly.Location);
            //Get the name of the requested assembly.
			var name = args.Name.Split (',') [0];
			//Get all the files in the requesting assembly's directory, recursively down.
            var baseDirFiles = Directory.EnumerateFiles (baseDir, "*", SearchOption.AllDirectories).ToList ();
            //For each file, see if its name contains the requested assembly's name.
			var foundFile = baseDirFiles.FirstOrDefault (p => p.Contains (name + ".dll"));
            if (foundFile == null) return null;
            var assembly = Assembly.LoadFrom (foundFile);
            return assembly;
        }

		/// <summary>
		/// Uninstalls a plugin specified by the given id.
		/// </summary>
		/// <param name="pluginId">Plugin identifier.</param>
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

		/// <summary>
		/// Registers dependencies for and then initializes all currently installed plugins.
		/// </summary>
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

		/// <summary>
		/// Checks whether the specified plugin is registered.
		/// </summary>
		/// <returns><c>true</c>, if registered, <c>false</c> otherwise.</returns>
		/// <param name="pluginId">Plugin identifier.</param>
        public bool CheckRegistered (Guid pluginId)
        {
            return _mgr.CheckRegistered (pluginId);
        }

		/// <summary>
		/// Checks whether the specified plugin type is registered.
		/// </summary>
		/// <returns><c>true</c>, if registered, <c>false</c> otherwise.</returns>
		/// <typeparam name="TPlugin">The 1st type parameter.</typeparam>
        public bool CheckRegistered<TPlugin> () where TPlugin : Plugin
        {
            return _mgr.CheckRegistered<TPlugin> ();
        }
    }
}

