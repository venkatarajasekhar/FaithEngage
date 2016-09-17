using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

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
            var filesNeeded = new[]
            {
                new templateDef{fname = "TextUnitEditorTemplate.cshtml", name = "TextUnitEditor"},
                new templateDef{fname = "TextUnitCardTemplate.cshtml", name="TextUnitCard" }
            };

            registerTemplates(factory, filesNeeded);

            var pluginContainer = factory
        }

        public void LoadBootstrappers(IBootList bootstrappers)
        {
        }

        public void RegisterDependencies(IRegistrationService regService)
        {
        }
    }
}

