using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.Factories;
using System.IO;
using System.Linq;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
    public abstract class DisplayUnitPlugin : Plugin
    {
		public DisplayUnitPlugin()
		{
			this.PluginType = PluginTypeEnum.DisplayUnit;
		}
		/// <summary>
		/// This is the Specific Type this plugin represents. 
		/// </summary>
		/// <value>The display type of the unit.</value>
		abstract public Type DisplayUnitType { get;}

        /// <summary>
        /// All display unit plugins must specify the names of the attributes they
        /// will use. These are used for DTO translation, instantiation, and Db storage.
        /// </summary>
        /// <returns>The attributes.</returns>
        abstract public List<string> GetAttributeNames();

        /// <summary>
        /// Defines how the editor or this plugin type will function.
        /// </summary>
        /// <value>The editor definition.</value>
        abstract public IDisplayUnitEditorDefinition EditorDefinition { get;}


        protected class templateDef
        {
            public string FileName {
                get;
                set;
            }

            public string TemplateName {
                get;
                set;
            }
        }

        protected void registerTemplates (IAppFactory factory, templateDef [] filesNeeded)
        {
            var tempService = factory.TemplatingService;
            var fileMgr = factory.PluginFileManager;
            var files = fileMgr.GetFilesForPlugin (this.PluginId.Value).Select (p => p.Value.FileInfo);

            var filesObtained =
                from f in files
                from fn in filesNeeded
                    where f.FullName.Contains (fn.FileName)
                select new{
                    fileInfo = f, key = fn.TemplateName
                };

            foreach (var file in filesObtained) {
                try {
                    var tempString = getTemplateString (file.fileInfo);
                    tempService.RegisterTemplate (tempString, file.key);
                } catch (Exception ex) {
                    continue;
                }
            }

        }

        private string getTemplateString (FileInfo file)
        {
            string templateString = null;
            using (var reader = file.OpenText ()) {
                templateString = reader.ReadToEnd ();
            }
            return templateString;
        }

    }
}

