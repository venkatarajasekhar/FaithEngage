using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.RepoManagers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.Bootstrappers;

namespace FaithEngage.Core.Userclasses
{
    public class UserClassBootstrapper : IBootstrapper
    {
        public void Execute (IAppFactory fac)
        {
        }

        public void LoadBootstrappers (IBootList bootstrappers)
        {
        }

        public void RegisterDependencies (IRegistrationService rs)
        {
			rs.Register<IOrganizationRepoManager, OrganizationRepoManager>(LifeCycle.Singleton);
			rs.Register<IUserRepoManager, UserRepoManager>(LifeCycle.Singleton);
        }
    }
}

