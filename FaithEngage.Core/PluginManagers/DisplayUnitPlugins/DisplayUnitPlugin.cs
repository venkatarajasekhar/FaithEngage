using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.DisplayUnitEditor;

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
        abstract public IDisplayUnitEditorDefinition EditorDefinition { get; set;}

    }
}

