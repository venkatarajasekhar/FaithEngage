using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.Files.Factories;
using FaithEngage.Core.PluginManagers.Files.Interfaces;

namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
            
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<IPluginFileManager, IPluginFileManager> (LifeCycle.Singleton);
            container.Register<IConverterFactory<PluginFileInfo, PluginFileInfoDTO>, PluginFileInfoDTOFactory> (LifeCycle.Transient);
            container.Register<IPluginFileInfoFactory, PluginFileInfoFactory> (LifeCycle.Transient);
        }
    }
}

