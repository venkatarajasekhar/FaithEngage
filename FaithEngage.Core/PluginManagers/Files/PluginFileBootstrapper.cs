using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.Files.Interfaces;

namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
            throw new NotImplementedException ();
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<IPluginFileManager, IPluginFileManager> (LifeCycle.Singleton);
        }
    }
}

