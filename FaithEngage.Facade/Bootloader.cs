using System;
using System.Collections.Generic;
using FaithEngage.Core;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Facade
{
    public class Bootloader : IBootstrapper
	{

        public void Execute (IAppFactory factory)
        {
        }

        public void LoadBootstrappers (IBootList bootstrappers)
        {
            bootstrappers.Load<FaithEngageBootLoader> ();
        }

        public void RegisterDependencies (IRegistrationService rs)
        {
            rs.Register<IAuthenticator, Authenticator> (LifeCycle.Transient);
        }
    }
}

