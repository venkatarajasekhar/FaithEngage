using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Events.Interfaces;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.CardProcessor;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;

namespace FaithEngage.Core.Factories
{
    public interface IAppFactory
    {
        IDisplayUnitsRepoManager DisplayUnitsRepo {get;}
        IDisplayUnitPluginRepoManager DisplayUnitsPluginRepo {get;}
        IOrganizationRepoManager OrganizationRepo { get; }
        IUserRepoManager UserRepo{ get; }
        IEventRepoManager EventRepo{ get; }
        IEventScheduleRepoManager EventScheduleRepo{ get; }
        ICardProcessor CardProcessor { get; }
        ITemplatingService TemplatingService{ get; }
        IPluginFileManager PluginFileManager{ get; }
    }
}

