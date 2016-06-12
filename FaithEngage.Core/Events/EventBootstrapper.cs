using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core
{
    public class EventBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<IEventRepoManager, EventRepoManager> (LifeCycle.Transient);
        }
    }
}

