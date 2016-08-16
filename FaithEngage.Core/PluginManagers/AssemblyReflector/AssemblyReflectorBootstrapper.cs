using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.AssemblyReflector.Interfaces;
using FaithEngage.Core.PluginManagers.AssemblyReflector;

namespace FaithEngage.Core
{
    public class AssemblyReflectorBootstrapper : IBootstrapper
    {

        public BootPriority BootPriority {
            get {
                return BootPriority.Last;
            }
        }

        public void Execute (IAppFactory factory)
        {
        }

        public void LoadBootstrappers (IBootList bootstrappers)
        {
        }

        public void RegisterDependencies (IRegistrationService regService)
        {
            regService.Register<IAssemblyReflectionMgr, AssemblyReflector> (LifeCycle.Transient);
        }
    }
}

