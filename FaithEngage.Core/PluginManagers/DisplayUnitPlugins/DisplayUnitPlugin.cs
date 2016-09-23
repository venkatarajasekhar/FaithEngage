using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.Factories;
using System.IO;
using System.Linq;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
    /// <summary>
    /// This is the class from which all DisplayUnitPlugins must derrive. It provides additional
	/// properties and methods used by DisplayUnitPlugins. 
    /// </summary>
	public abstract class DisplayUnitPlugin : Plugin
    {
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:FaithEngage.Core.PluginManagers.DisplayUnitPlugins.DisplayUnitPlugin"/> class.
		/// </summary>
		public DisplayUnitPlugin()
		{
			//Set the pluginType as DisplayUnit.
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

		/// <summary>
		/// Used to register templates to a DisplayUnitPlugin.
		/// </summary>
        protected class templateDef
        {
			/// <summary>
			/// Gets or sets the file name (with extension)
			/// </summary>
			/// <value>The name of the file.</value>
			public string FileName {
                get;
                set;
            }
			/// <summary>
			/// Gets or sets the name of the template to be registered, used as the template key.
			/// </summary>
			/// <value>The name of the template.</value>
            public string TemplateName {
                get;
                set;
            }
        }
		/// <summary>
		/// Registers the templates templates specified for later access by the plugin. 
		/// </summary>
		/// <remarks>Best called within the initialize function. </remarks>
		/// <param name="factory">Factory.</param>
		/// <param name="filesNeeded">An array of template definitions.</param>
        protected void registerTemplates (IAppFactory factory, templateDef [] filesNeeded)
        {
            //Get the templating service
			var tempService = factory.TemplatingService;
            //Get the file mnager
			var fileMgr = factory.PluginFileManager;
			//Use the filemanager to get this plugin's files
			var files = fileMgr.GetFilesForPlugin (this.PluginId.Value)
			                   .Select (p => p.Value.FileInfo);
			//Project files and filesNeeded into ienumerable anonymous object
            var filesObtained =
                from f in files
                from fn in filesNeeded
                    //Only get those files needed actually contained in plugin files
					where f.FullName.Contains (fn.FileName)
                select new{
                    fileInfo = f, key = fn.TemplateName
                };
			//Loop through the new anonymous object
            foreach (var file in filesObtained) {
                try {
                    //Read the template into a string
					var tempString = getTemplateString (file.fileInfo);
                    //Register the template to the templating service.
					tempService.RegisterTemplate (tempString, file.key);
                } catch (Exception ex) {//In the event of an exception, skip it and continue.
                    continue;
                }
            }

        }
		/// <summary>
		/// Reads a template text file into a string.
		/// </summary>
		/// <returns>The template string.</returns>
		/// <param name="file">The text file.</param>
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

