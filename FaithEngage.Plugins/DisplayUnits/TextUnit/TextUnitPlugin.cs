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

        private ITemplatingService _tempService;
        private IDictionary<Guid, PluginFileInfo> _files;


        public override void Initialize (IAppFactory FEFactory)
        {
            var fileMgr = FEFactory.PluginFileManager;
            _tempService = FEFactory.TemplatingService;
            _files = fileMgr.GetFilesForPlugin (this.PluginId.Value);
            var editorTemplate = getTemplateString ("TextUnitEditorTemplate.cshtml");
            registerTemplate ("TextUnitEditor", editorTemplate);
            var cardTemplate = getTemplateString ("TextUnitCardTemplate.cshtml");
            registerTemplate ("TextUnitCard", cardTemplate);
        }

        private string getTemplateString(string fileName){
            var template = _files.FirstOrDefault (p => p.Value.FileInfo.Name == fileName);
            FileInfo obtainedFile = null;
            if (template.Value != null) return null;
            obtainedFile = template.Value.FileInfo;
            obtainedFile.Refresh ();
            if (!obtainedFile.Exists) return null;
            string templateString = null;
            using (var reader = obtainedFile.OpenText ()) {
                templateString = reader.ReadToEnd ();
            }
            return templateString;
        }

        private void registerTemplate(string templateName, string template)
        {
            if (!string.IsNullOrWhiteSpace (template)) return;
            _tempService.RegisterTemplate (template, templateName);
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

