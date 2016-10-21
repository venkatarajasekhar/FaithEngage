using System;
using System.Threading;
namespace FaithEngage.Core.PluginManagers
{
    /// <summary>
    /// A Basic info class to store info about a plugin package. This is serialized to and from JSON.
    /// </summary>
	public class PluginPackage
    {

        public class pluginInfo
        {
            /// <summary>
            /// Gets or sets the name of the plugin type.
            /// </summary>
            /// <value>The name of the plugin type.</value>
			public string PluginTypeName { get; set; }
            /// <summary>
            /// Gets or sets the plugin type enum.
            /// </summary>
            /// <value>The plugin type enum.</value>
			public PluginTypeEnum PluginTypeEnum { get; set; }
            /// <summary>
            /// Gets or sets the name of the dll.
            /// </summary>
            /// <value>The name of the dll.</value>
			public string DllName { get; set;}
            /// <summary>
            /// Gets or sets an array of file names associated with a given plugin in the package.
            /// </summary>
            /// <value>The files.</value>
			public string [] Files { get; set;}
        }
		/// <summary>
		/// Gets or sets an array of plugins
		/// </summary>
		/// <value>The plugins.</value>
        public pluginInfo[] Plugins {
            get;
            set;
        }



    }
}

