﻿using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using FaithEngage.Core.Factories;
using System.IO;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using FaithEngage.Core.Exceptions;
using System.Security;

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
                throw new PluginLoadException ($"An IO Exception occurred reading the file: {pluginsFile.Name}");
            }

            int num = 0;
            foreach (var pinfo in pPackage.Plugins){
                var plugId = storeFilesForPlugin (pinfo, files);
                var dll = getDll (pinfo, plugId);
                if (dll == null){
                    _fileMgr.FlushTempFolder (key);
                    _fileMgr.DeleteAllFilesForPlugin (plugId);
                    throw new PluginLoadException ("Dll not found for plugin: " + pinfo.PluginTypeName);
                }
                var plugin = getPlugin (pinfo, dll);
                num++;
                _mgr.RegisterNew (plugin, plugId);
                plugin.Install (_factory);
            }
            _fileMgr.FlushTempFolder (key);
            return num;
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
            _fileMgr.StoreFilesForPlugin (pFiles.ToList (), plugId, true);
            return plugId;
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

        public void Uninstall(Guid pluginId)
		{
            _mgr.UninstallPlugin (pluginId);
		}
	}
}

