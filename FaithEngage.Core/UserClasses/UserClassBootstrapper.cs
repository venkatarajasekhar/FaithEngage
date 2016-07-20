using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.RepoManagers;

namespace FaithEngage.Core.Userclasses
{
    public class UserClassBootstrapper : IBootstrapper
    {
        public void Execute (IContainer container)
        {
        }

        public void LoadBootstrappers (IList<IBootstrapper> bootstrappers)
        {
        }

        public void RegisterDependencies (IContainer container)
        {
			container.Register<IOrganizationRepoManager, OrganizationRepoManager>(LifeCycle.Singleton);
			container.Register<IUserRepoManager, UserRepoManager>(LifeCycle.Singleton);
        }
    }
}

