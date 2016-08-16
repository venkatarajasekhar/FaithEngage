using System;
using System.Reflection;
using System.IO;
using FaithEngage.Core.PluginManagers.AssemblyReflector.Interfaces;
using System.Linq;
namespace FaithEngage.Core
{
    /// <summary>
    /// Assembly reflection proxy.
    /// Derrived from the code sample here:
    /// http://www.codeproject.com/Articles/453778/Loading-Assemblies-from-Anywhere-into-a-New-AppDom
    /// </summary>
    public class AssemblyReflectionProxy : MarshalByRefObject
    {
        private string _assemblyPath;

        public void LoadAssembly (string assemblyPath)
        {
            _assemblyPath = assemblyPath;
            try{
                Assembly.ReflectionOnlyLoadFrom (assemblyPath);
            }
            catch (FileNotFoundException){
                // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain.
            }
        }

        public TResult Reflect<TResult> (Func<Assembly, TResult> func)
        {
            DirectoryInfo directory = new FileInfo (_assemblyPath).Directory;
            ResolveEventHandler resolveEventHandler = (s, e) => {
                return OnReflectionOnlyResolve (e, directory);
            };
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

            var assembly = AppDomain.CurrentDomain
                                    .ReflectionOnlyGetAssemblies ()
                                    .FirstOrDefault (
                                        a => a.Location.CompareTo (_assemblyPath) == 0
                                       );
            var result = func (assembly);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;
            return result;
        }

        private Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
        {
            //First, check all currently loaded assemblies for this name, if found, return it.
            Assembly loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies ()
                                               .FirstOrDefault (
                                                   asm => string.Equals (
                                                       asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase)
                                                  );
            if (loadedAssembly != null) return loadedAssembly;

            //Check the directory given for the assembly name. If found, return it.
            AssemblyName assemblyName = new AssemblyName (args.Name);
            string dependentAssemblyFilename = Path.Combine (directory.FullName, assemblyName.Name + ".dll");

            if (File.Exists (dependentAssemblyFilename)) {
                return Assembly.ReflectionOnlyLoadFrom (
                    dependentAssemblyFilename);
            }
            //Otherwise, try to load from the current working directory (likely to fail at this point).
            return Assembly.ReflectionOnlyLoad (args.Name);
        }
    }
}

