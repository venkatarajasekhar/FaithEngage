using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.Factories
{
    public class AppFactory : IAppFactory
    {
        private readonly IContainer _container;

        public AppFactory(IContainer container)
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

        public IEventScheduleRepoManager EventScheduleRepo{
            get{
                return _container.Resolve<IEventScheduleRepoManager> ();
            }
        }

        public ICardProcessor CardProcessor{
            get{
                return _container.Resolve<ICardProcessor> ();
            }
        }

        public ITemplatingService TemplatingService{
            get{
                return _container.Resolve<ITemplatingService> ();
            }
        }

        public IPluginFileManager PluginFileManager{
            get{
                return _container.Resolve<IPluginFileManager> ();
            }
        }

		public IConfigRepository ConfigManager
		{
			get
			{
				return _container.Resolve<IConfigRepository>();
			}
		}

		public T GetOther<T>()
		{
			if (typeof(T) == typeof(IContainer)) throw new FactoryException("Accessing the IContainer from the AppFactory is not permitted.");
			return _container.Resolve<T>();
		}
	}
}

