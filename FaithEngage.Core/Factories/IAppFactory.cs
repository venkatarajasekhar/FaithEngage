using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Containers;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.Factories
{
    /// <summary>
    /// This is the centralized means by which the application's registered dependencies can be accessed, particularly
	/// within bootstrappers and within plugins.
    /// </summary>
	public interface IAppFactory
    {
        /// <summary>
		/// Obtains the application's DisplayUnitsRepositoryManager
		/// </summary>
		/// <value>The display units repo.</value>
		IDisplayUnitsRepoManager DisplayUnitsRepo {get;}
        /// <summary>
		/// Gets the application's DisplayUnitPluginRepositoryManager
		/// </summary>
		/// <value>The display units plugin repo.</value>
		IDisplayUnitPluginRepoManager DisplayUnitsPluginRepo {get;}
        /// <summary>
		/// Gets the application's OrganizationRepositoryManager
		/// </summary>
		/// <value>The organization repo.</value>
		IOrganizationRepoManager OrganizationRepo { get; }
        /// <summary>
		/// Gets the application's UserRepositoryManager
		/// </summary>
		/// <value>The user repo.</value>
		IUserRepoManager UserRepo{ get; }
        /// <summary>
		/// Gets the application's EventRepositoryManager
		/// </summary>
		/// <value>The event repo.</value>
		IEventRepoManager EventRepo{ get; }
        /// <summary>
		/// Gets the application's EventScheduleRepositoryManager
		/// </summary>
		/// <value>The event schedule repo.</value>
		IEventScheduleRepoManager EventScheduleRepo{ get; }
        /// <summary>
		/// Gets the application's CardProcessor
		/// </summary>
		/// <value>The card processor.</value>
		ICardProcessor CardProcessor { get; }
        /// <summary>
		/// Gets the application's TemplatingService
		/// </summary>
		/// <value>The templating service.</value>
		ITemplatingService TemplatingService{ get; }
        /// <summary>
		/// Gets the application's PluginFileManager
		/// </summary>
		/// <value>The plugin file manager.</value>
		IPluginFileManager PluginFileManager{ get; }
		/// <summary>
		/// Get's the applications ConfigurationManager.
		/// </summary>
		/// <value>The config manager.</value>
		IConfigRepository ConfigManager { get; }
        /// <summary>
		/// Gets the application's RegistrationService
		/// </summary>
		/// <value>The registration service.</value>
		IRegistrationService RegistrationService { get; }
		/// <summary>
		/// Gets the application's PluginManager
		/// </summary>
		/// <value>The plugin manager.</value>
		IPluginManager PluginManager { get; }
		/// <summary>
		/// Obtains any other kind of requested dependency from the application, EXCEPT the application's IContainer,
		/// which is not permitted for security reasons.
		/// </summary>
		/// <returns>The other.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		T GetOther<T>();
    }
}

