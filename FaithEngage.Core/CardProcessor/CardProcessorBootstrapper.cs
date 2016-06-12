using System.Collections.Generic;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core.CardProcessor
{
    public class CardProcessorBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<ICardProcessor, CardProcessor> (LifeCycle.Singleton);
        }
    }
}

