using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Cards.Interfaces;

namespace FaithEngage.Core.Cards
{
    public class CardBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<ICardDTOFactory, CardDtoFactory> (LifeCycle.Transient);
        }
    }
}

