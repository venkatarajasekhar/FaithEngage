using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.Linq;

namespace FaithEngage.Plugins.DisplayUnits.TextUnit
{
    public class TextUnitPlugin : DisplayUnitPlugin
    {
        public TextUnitPlugin ()
        {
            _attributeNames = new List<string> { "Text" };
        }
        public override Type DisplayUnitType {
            get {
                return typeof (TextUnit);
            }
        }

        public override IDisplayUnitEditorDefinition EditorDefinition {
            get {
                return new TextUnitEditorDefinition ();
            }
        }

        public override string PluginName {
            get {
                return "Text Block";
            }
        }

        public override int [] PluginVersion {
            get {
                return new int [] { 1, 0, 0};
            }
        }

        private List<string> _attributeNames;
        public override List<string> GetAttributeNames ()
        {
            return _attributeNames;
        }

        public override void Initialize (IAppFactory FEFactory)
        {
            var fileMgr = FEFactory.PluginFileManager;
            var tempService = FEFactory.TemplatingService;
            var files = fileMgr.GetFilesForPlugin (this.PluginId.Value);
            var editorTemplate = 
                files
                    .FirstOrDefault (p => p.Value.FileInfo.Name == "TextUnitEditorTemplate.cshtml")
                    .Value
                    .FileInfo;
            string template;
            using (var reader = editorTemplate.OpenText ())
            {
                template = reader.ReadToEnd ();
            }
            tempService.RegisterTemplate (template, "TextUnitEditor");
            var cardTemplate = files.FirstOrDefault (p => p.Value.FileInfo.Name == "TextUnitCard.cshtml").Value.FileInfo;
            string card;
            using (var reader = cardTemplate.OpenText())
            {
                card = reader.ReadToEnd ();
            }
            tempService.RegisterTemplate (card, "TextUnitCard");
        }

        public override void Install (IAppFactory FEFactory)
        {
        }

        public override void RegisterDependencies (IRegistrationService regService)
        {
        }

        public override void Uninstall (IAppFactory FEFactory)
        {
        }
    }
}

