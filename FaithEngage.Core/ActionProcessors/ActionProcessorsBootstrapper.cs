using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.ActionProcessors.Interfaces;
using FaithEngage.Core.ActionProcessors;

namespace FaithEngage.Core
{
    public class ActionProcessorsBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<ICardActionProcessor, CardActionProcessor> (LifeCycle.Singleton);
        }
    }
}

