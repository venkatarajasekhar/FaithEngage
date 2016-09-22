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
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.Factories
{
    /// <summary>
    /// This is the centralized means by which the application's registered dependencies can be accessed, particularly
	/// within bootstrappers and within plugins.
    /// </summary>
	public class AppFactory : IAppFactory
    {
        private readonly IContainer _container;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Factories.AppFactory"/> class.
		/// </summary>
		/// <param name="container">Container.</param>
        public AppFactory(IContainer container)
        {
            _container = container;
        }

		/// <summary>
		/// Obtains the application's DisplayUnitsRepositoryManager
		/// </summary>
		/// <value>The display units repo.</value>
        public IDisplayUnitsRepoManager DisplayUnitsRepo {
            get {
                return _container.Resolve<IDisplayUnitsRepoManager> ();
            }
        }
		/// <summary>
		/// Gets the application's DisplayUnitPluginRepositoryManager
		/// </summary>
		/// <value>The display units plugin repo.</value>
        public IDisplayUnitPluginRepoManager DisplayUnitsPluginRepo {
            get {
                return _container.Resolve<IDisplayUnitPluginRepoManager> ();
            }
        }
		/// <summary>
		/// Gets the application's OrganizationRepositoryManager
		/// </summary>
		/// <value>The organization repo.</value>
        public IOrganizationRepoManager OrganizationRepo {
            get {
                return _container.Resolve<IOrganizationRepoManager> ();
            }
        }
		/// <summary>
		/// Gets the application's UserRepositoryManager
		/// </summary>
		/// <value>The user repo.</value>
        public IUserRepoManager UserRepo {
            get {
                return _container.Resolve<IUserRepoManager> ();
            }
        }
		/// <summary>
		/// Gets the application's EventRepositoryManager
		/// </summary>
		/// <value>The event repo.</value>
        public IEventRepoManager EventRepo {
            get {
                return _container.Resolve<IEventRepoManager> ();
            }
        }
		/// <summary>
		/// Gets the application's EventScheduleRepositoryManager
		/// </summary>
		/// <value>The event schedule repo.</value>
        public IEventScheduleRepoManager EventScheduleRepo{
            get{
                return _container.Resolve<IEventScheduleRepoManager> ();
            }
        }
		/// <summary>
		/// Gets the application's CardProcessor
		/// </summary>
		/// <value>The card processor.</value>
        public ICardProcessor CardProcessor{
            get{
                return _container.Resolve<ICardProcessor> ();
            }
        }
		/// <summary>
		/// Gets the application's TemplatingService
		/// </summary>
		/// <value>The templating service.</value>
        public ITemplatingService TemplatingService{
            get{
                return _container.Resolve<ITemplatingService> ();
            }
        }
		/// <summary>
		/// Gets the application's PluginFileManager
		/// </summary>
		/// <value>The plugin file manager.</value>
        public IPluginFileManager PluginFileManager{
            get{
                return _container.Resolve<IPluginFileManager> ();
            }
        }
		/// <summary>
		/// Get's the applications ConfigurationManager.
		/// </summary>
		/// <value>The config manager.</value>
		public IConfigRepository ConfigManager
		{
			get
			{
				return _container.Resolve<IConfigRepository>();
			}
		}
		/// <summary>
		/// Gets the application's RegistrationService
		/// </summary>
		/// <value>The registration service.</value>
        public IRegistrationService RegistrationService {
            get {
                return _container.GetRegistrationService ();
            }
        }
		/// <summary>
		/// Gets the application's PluginManager
		/// </summary>
		/// <value>The plugin manager.</value>
		public IPluginManager PluginManager
		{
			get
			{
				return _container.Resolve<IPluginManager>();
			}
		}
		/// <summary>
		/// Obtains any other kind of requested dependency from the application, EXCEPT the application's IContainer,
		/// which is not permitted for security reasons.
		/// </summary>
		/// <returns>The other.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetOther<T>()
		{
			if (typeof(T) == typeof(IContainer)) throw new FactoryException("Accessing the IContainer from the AppFactory is not permitted.");
			return _container.Resolve<T>();
		}
	}
}

