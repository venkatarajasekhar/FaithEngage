using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FaithEngage.Core.PluginManagers.AssemblyReflector.Interfaces;
using System.Linq;
using System.Security.Policy;

namespace FaithEngage.Core.PluginManagers.AssemblyReflector
{
    public class AssemblyReflector : IAssemblyReflectionMgr
    {
        Dictionary<string, AppDomain> _mapDomains = new Dictionary<string, AppDomain> ();
        Dictionary<string, AppDomain> _loadedAssemblies = new Dictionary<string, AppDomain> ();
        Dictionary<string, AssemblyReflectionProxy> _proxies = new Dictionary<string, AssemblyReflectionProxy> ();

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        ~AssemblyReflector()
        {
            Dispose ();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                foreach (var appDomain in _mapDomains.Values)
                    AppDomain.Unload (appDomain);
                _loadedAssemblies.Clear ();
                _proxies.Clear ();
                _mapDomains.Clear ();
            }
        }

        public bool LoadAssembly (string assemblyPath, string domainName)
        {
            // if the assembly file does not exist then fail
            if (!File.Exists (assemblyPath))
                return false;

            // if the assembly was already loaded then fail
            if (_loadedAssemblies.ContainsKey (assemblyPath)) {
                return false;
            }

            // check if the appdomain exists, and if not create a new one
            AppDomain appDomain = null;
            if (_mapDomains.ContainsKey (domainName)) {
                appDomain = _mapDomains [domainName];
            } else {
                appDomain = CreateChildDomain (AppDomain.CurrentDomain, domainName);
                _mapDomains [domainName] = appDomain;
            }

            // load the assembly in the specified app domain
             try
             {
                Type proxyType = typeof(AssemblyReflectionProxy);
                if (proxyType.Assembly != null)
                {
                    //Create an instance of the AssemblyRefelectionProxy in the new AppDomain
                    //And save the reference to the proxy variable.
                   var proxy =
                   (AssemblyReflectionProxy)appDomain.
                       CreateInstanceFromAndUnwrap(
                       proxyType.Assembly.Location,
                       proxyType.FullName);
                    //Use that proxy (now shared by reference between the otherwise isolated appDomains) to load
                    //the specified assembly into its appdomain.
                    proxy.LoadAssembly(assemblyPath);
                    //Map the assemblyPath to the new appDomain & save it.
                    _loadedAssemblies[assemblyPath] = appDomain;
                    //Map the assemblyPath to the new proxy & save it.
                    _proxies[assemblyPath] = proxy;
                    return true;
                }
             }
             catch
             {}

             return false;
        }

        public TResult Reflect<TResult> (string assemblyPath, Func<Assembly, TResult> func)
        {
            // check if the assembly is found in the internal dictionaries
            if (_loadedAssemblies.ContainsKey (assemblyPath) && _proxies.ContainsKey (assemblyPath)) {
                return _proxies [assemblyPath].Reflect (func);
            }

            return default (TResult);
        }

        public bool UnloadAssembly (string assemblyPath)
        {
            if (!File.Exists (assemblyPath))
                return false;
            // check if the assembly is found in the internal dictionaries
            if (_loadedAssemblies.ContainsKey (assemblyPath) && _proxies.ContainsKey (assemblyPath)) {
                // check if there are more assemblies loaded in the same app domain; in this case fail
                AppDomain appDomain = _loadedAssemblies [assemblyPath];
                int count = _loadedAssemblies.Values.Count (a => a == appDomain);
                if (count != 1)
                    return false;

                try {
                    // remove the appdomain from the dictionary and unload it from the process
                    _mapDomains.Remove (appDomain.FriendlyName);
                    AppDomain.Unload (appDomain);

                    // remove the assembly from the dictionaries
                    _loadedAssemblies.Remove (assemblyPath);
                    _proxies.Remove (assemblyPath);

                    return true;
                } catch {
                }
            }

            return false;
        }

        public bool UnloadDomain (string domainName)
        {
            // check the appdomain name is valid
            if (string.IsNullOrEmpty (domainName))
                return false;

            // check we have an instance of the domain
            if (_mapDomains.ContainsKey (domainName)) {
                try {
                    var appDomain = _mapDomains [domainName];

                    // check the assemblies that are loaded in this app domain
                    var assemblies = new List<string> ();
                    foreach (var kvp in _loadedAssemblies) {
                        if (kvp.Value == appDomain)
                            assemblies.Add (kvp.Key);
                    }

                    // remove these assemblies from the internal dictionaries
                    foreach (var assemblyName in assemblies) {
                        _loadedAssemblies.Remove (assemblyName);
                        _proxies.Remove (assemblyName);
                    }

                    // remove the appdomain from the dictionary
                    _mapDomains.Remove (domainName);

                    // unload the appdomain
                    AppDomain.Unload (appDomain);

                    return true;
                } catch {
                }
            }

            return false;
        }

        private AppDomain CreateChildDomain (AppDomain parentDomain, string domainName)
        {
            Evidence evidence = new Evidence (parentDomain.Evidence);
            AppDomainSetup setup = parentDomain.SetupInformation;
            return AppDomain.CreateDomain (domainName, evidence, setup);
        }

    }
}

