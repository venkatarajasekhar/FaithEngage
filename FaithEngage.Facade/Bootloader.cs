using System;
using System.Collections.Generic;
using FaithEngage.Core;
using FaithEngage.Core.Containers;

namespace FaithEngage.Facade
{
    public class Bootloader : IBootstrapper
	{
        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
            var feBooter = new FaithEngageBootLoader ();

            bootstrappers.Add (feBooter);

            feBooter.LoadBootstrappers (bootstrappers);
        }

        public void RegisterDependencies (IContainer container)
        {
            container.Register<IAuthenticator, Authenticator> (LifeCycle.Transient);
        }
    }
}

