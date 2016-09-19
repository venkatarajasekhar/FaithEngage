using System.Linq;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
{
    public class TextUnitBootstrapper : DisplayUnitsBootstrapper
    {
        public BootPriority BootPriority
        {
            get
            {
                return BootPriority.Normal;
            }
        }

        public void Execute(IAppFactory factory)
        {
			var plugMgr = factory.PluginManager;
            if (!plugMgr.CheckRegistered<TextUnitPlugin>()) install(plugMgr);
        } 

        private void install(IPluginManager pluginManager)
        {
            var files = this.pluginFiles
                .Where(p => p.Name == "TextUnitEditorTemplate.cshtml" || p.Name == "TextUnitCardTemplate.cshtml")
                .ToList();
            pluginManager.Install<TextUnitPlugin>(files);
        }

        public void LoadBootstrappers(IBootList bootstrappers)
        {
        }

        public void RegisterDependencies(IRegistrationService regService)
        {
        }
    }
}

