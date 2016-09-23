using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	/// <summary>
	/// A centralized container to keep all DisplayUnitPlugin types in memory, as needed.
	/// </summary>
	public class DisplayUnitPluginContainer : IDisplayUnitPluginContainer
    {
		public DisplayUnitPluginContainer ()
		{
			_registry = new Dictionary<Guid, DisplayUnitPlugin> ();
		}
		private readonly Dictionary<Guid, DisplayUnitPlugin> _registry;

		 /// <summary>
        /// Register the specified plugin for later access.
        /// </summary>
        /// <param name="plugin">Plugin.</param>
        public void Register(DisplayUnitPlugin plugin)
        {
			//If the plugin has an empty or null id, throw
			if (!plugin.PluginId.HasValue || plugin.PluginId.Value == Guid.Empty)
				throw new PluginHasInvalidIdException ("PluginId must be valid and not null.");
			//If plugin's display unit type doesn't have the two kosher constructors, throw
			if (!hasProperConstructors (plugin))
                throw new PluginHasInvalidConstructorsException (
                    "Plugin DisplayUnit type needs to support the " +
                    "the constructor signatures of the DisplayUnit base class.");
            //If the plugin's display unit type isn't derrived from DisplayUnit, throw.
			if (!plugin.DisplayUnitType.IsSubclassOf(typeof(DisplayUnit)))
                throw new NotDisplayUnitException (plugin.DisplayUnitType, "DisplayUnitType is not derived from DisplayUnit");
            try {
				//Add the plugin to the registry with the plugin's ID as the value.
				_registry.Add (plugin.PluginId.Value, plugin);
            } catch (ArgumentException) {//Don't re-register a plugin.
                throw new PluginAlreadyRegisteredException ();
            }
        }
		/// <summary>
        /// Resolve the specified PluginId to a DisplayUnitPlugin
        /// </summary>
        /// <param name="PluginId">Plugin identifier.</param>
        public DisplayUnitPlugin Resolve(Guid PluginId)
        {
            DisplayUnitPlugin plugin;
            if(_registry.TryGetValue(PluginId, out plugin))
            {
                return plugin;
            }
            return null;
        }
		/// <summary>
		/// Checks the plugin's DisplayUnitType for 2 constructors: One with a Dictionary&lt;string,string>
		/// and another with a Guid, Dictionary&lt;string,string>.
		/// </summary>
		/// <returns><c>true</c>, if proper constructors are present, <c>false</c> otherwise.</returns>
		/// <param name="plugin">Plugin.</param>
        private bool hasProperConstructors(DisplayUnitPlugin plugin)
        {
            var params1 = new Type[]{ typeof(Dictionary<string,string>) };
            var params2 = new Type[] {
                typeof(Guid),
                typeof(Dictionary<string,string>)
            };
            bool passes = false;
            try {
                var ctor1 = plugin.DisplayUnitType.GetConstructor (params1);
                var ctor2 = plugin.DisplayUnitType.GetConstructor (params2);
                passes = (ctor1 != null && ctor2 !=null);
            } catch {
                passes = false;
            }
            return passes;


        }
    }
}

