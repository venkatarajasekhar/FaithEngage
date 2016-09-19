using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.Linq;
using System.IO;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.TemplatingService;

namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
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
            var files = fileMgr.GetFilesForPlugin(this.PluginId.Value).Select(p => p.Value.FileInfo);

            var allFiles = files.Where(p =>
            {
                p.Refresh();
                if (p.Exists) return true;
                return false;
            });
            var filesNeeded = allFiles.Select(
                p => new templateDef { 
                    FileName = p.Name, 
                    TemplateName = p.Name.Replace("Template.cshtml", "") 
            }).ToArray();

            this.registerTemplates(FEFactory, filesNeeded);
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