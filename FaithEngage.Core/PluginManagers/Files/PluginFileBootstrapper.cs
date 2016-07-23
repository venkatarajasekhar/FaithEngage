using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.Files.Factories;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileBootstrapper : IBootstrapper
    {
        public void Execute (IAppFactory fac)
        {
            
        }

        public void LoadBootstrappers (IBootList bootstrappers)
        {
        }

        public void RegisterDependencies (IRegistrationService rs)
        {
            rs.Register<IPluginFileManager, IPluginFileManager> (LifeCycle.Singleton);
            rs.Register<IConverterFactory<PluginFileInfo, PluginFileInfoDTO>, PluginFileInfoDTOFactory> (LifeCycle.Transient);
            rs.Register<IPluginFileInfoFactory, PluginFileInfoFactory> (LifeCycle.Transient);
        }
    }
}

