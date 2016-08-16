using System;
using System.Reflection;

namespace FaithEngage.Core.PluginManagers.AssemblyReflector.Interfaces
{
    /// <summary>
    /// Assembly reflection mgr.
    /// This interface is derived from the code sample here:
    /// http://www.codeproject.com/Articles/453778/Loading-Assemblies-from-Anywhere-into-a-New-AppDom
    /// </summary>
    public interface IAssemblyReflectionMgr : IDisposable
    {
        bool LoadAssembly (string assemblyPath, string domainName);
        bool UnloadAssembly (string assemblyPath);
        bool UnloadDomain (string domainName);
        TResult Reflect<TResult> (string assemblyPath, Func<Assembly, TResult> func);

    }
}

