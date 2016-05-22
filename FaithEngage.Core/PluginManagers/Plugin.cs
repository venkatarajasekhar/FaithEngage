using System;

namespace FaithEngage.Core.PluginManagers
{
    public abstract class Plugin
    {
        /// <summary>
        /// This is the human-readable name of the plugin.
        /// </summary>
        /// <value>The name of the plugin.</value>
        abstract public string PluginName { get;}

        /// <summary>
        /// This is the full Assembly-qualified name of this plugin, used to uniquely identify each plugin.
        /// </summary>
        /// <value>The plugin identifier.</value>
        public string PluginId 
        { 
            get 
            {
                return this.GetType ().AssemblyQualifiedName;
            }
        }

        abstract public string PluginVersion{ get;}

        protected Plugin(){}

    }
}

