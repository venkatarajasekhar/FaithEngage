using System;
using System.Collections.Generic;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.CardProcessor
{
    public class CardProcessorBootstrapper : IBootstrapper
    {
        public BootPriority BootPriority {
            get {
                return BootPriority.Normal;
            }
        }

        public void Execute (IAppFactory container)
        {
        }

        public void LoadBootstrappers (IBootList bootstrappers)
        {
        }

        public void RegisterDependencies (IRegistrationService rs)
        {
            rs.Register<ICardProcessor, CardProcessor> ();
        }
    }
}

