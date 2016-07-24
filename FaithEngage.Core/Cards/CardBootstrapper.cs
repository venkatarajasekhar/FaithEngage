using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.Factories;
using FaithEngage.Core.Bootstrappers;

namespace FaithEngage.Core.Cards
{
    public class CardBootstrapper : IBootstrapper
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
            rs.Register<ICardDTOFactory, CardDtoFactory> (LifeCycle.Transient);
        }
    }
}

