using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;

namespace FaithEngage.Core.Factories
{
    public class AppFactory : IAppFactory
    {
        private readonly IContainer _container;

        private AppFactory(IContainer container)
        {
            _container = container;
        }

        public IDisplayUnitsRepoManager DisplayUnitsRepo {
            get {
                return _container.Resolve<IDisplayUnitsRepoManager> ();
            }
        }

        public IDisplayUnitPluginRepoManager DisplayUnitsPluginRepo {
            get {
                return _container.Resolve<IDisplayUnitPluginRepoManager> ();
            }
        }

        public IOrganizationRepoManager OrganizationRepo {
            get {
                return _container.Resolve<IOrganizationRepoManager> ();
            }
        }

        public IUserRepoManager UserRepo {
            get {
                return _container.Resolve<IUserRepoManager> ();
            }
        }

        public IEventRepoManager EventRepo {
            get {
                return _container.Resolve<IEventRepoManager> ();
            }
        }
    }
}

