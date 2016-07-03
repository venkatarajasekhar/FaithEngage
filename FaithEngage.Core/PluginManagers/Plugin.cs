using System;
using FaithEngage.Core.Containers;

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
        public Guid? PluginId { get; set; }

        public string FullName
        {
            get{
                return this.GetType ().FullName;
            }
        }

        public virtual string AssemblyLocation{
            get{
                return this.GetType ().Assembly.Location;
            }
        }

        abstract public int[] PluginVersion{ get;}

        abstract public void Initialize (IContainer container);
        abstract public void Install (IContainer container);
        abstract public void Uninstall (IContainer container);
        abstract public void RegisterDependencies (IContainer container);

        protected Plugin(){}

    }
}

