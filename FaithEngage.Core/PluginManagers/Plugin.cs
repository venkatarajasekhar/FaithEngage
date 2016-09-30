using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.Collections.Generic;
using System.IO;

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

		/// <summary>
		/// Gets the Plugin Type's full name, obtained through reflection.
		/// </summary>
		/// <value>The full name.</value>
        public string FullName
        {
            get{
                return this.GetType ().FullName;
            }
        }
		/// <summary>
		/// Gets current assembly's location of the type, obtained through reflection.
		/// </summary>
		/// <value>The assembly location.</value>
        public virtual string AssemblyLocation{
            get{
                return this.GetType ().Assembly.Location;
            }
        }
		/// <summary>
		/// Gets or sets the type of the plugin.
		/// </summary>
		/// <value>The type of the plugin.</value>
		public PluginTypeEnum PluginType { get; set; }
		/// <summary>
		/// Gets the plugin version.
		/// </summary>
		/// <value>The plugin version.</value>
        abstract public int[] PluginVersion{ get;}
		/// <summary>
		/// Initializes the plugin, so that it can be used.
		/// </summary>
		/// <param name="FEFactory">FEF actory.</param>
        abstract public void Initialize (IAppFactory FEFactory);
		/// <summary>
		/// Installs the plugin.
		/// </summary>
		/// <param name="FEFactory">FEF actory.</param>
		abstract public void Install (IAppFactory FEFactory);
		/// <summary>
		/// Uninstalls the plugin.
		/// </summary>
		/// <param name="FEFactory">FEF actory.</param>
		abstract public void Uninstall (IAppFactory FEFactory);
		/// <summary>
		/// Registerss dependencies for the plugin, so that it can be initialized.
		/// </summary>
		/// <param name="regService">Reg service.</param>
		abstract public void RegisterDependencies (IRegistrationService regService);
        //TODO: This really should be removed. It doesn't really serve a purpose.
		protected Plugin(){}

    }
}

