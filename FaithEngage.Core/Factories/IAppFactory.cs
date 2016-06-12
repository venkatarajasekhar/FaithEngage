using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.Events.Interfaces;

namespace FaithEngage.Core.Factories
{
    public interface IAppFactory
    {
        IDisplayUnitsRepoManager DisplayUnitsRepo { get; }
        IDisplayUnitPluginRepoManager DisplayUnitsPluginRepo { get; }
        IOrganizationRepoManager OrganizationRepo { get; }
        IUserRepoManager UserRepo { get; }
        IEventRepoManager EventRepo { get; }
    }
}

