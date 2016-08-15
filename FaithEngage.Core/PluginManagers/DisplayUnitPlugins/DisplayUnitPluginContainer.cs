using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
	public class DisplayUnitPluginContainer : IDisplayUnitPluginContainer
    {

        private readonly Dictionary<Guid,DisplayUnitPlugin> _registry = 
            new Dictionary<Guid, DisplayUnitPlugin> ();


        public void Register(DisplayUnitPlugin plugin)
        {
			if (!plugin.PluginId.HasValue || plugin.PluginId.Value == Guid.Empty)
				throw new PluginHasInvalidIdException ("PluginId must be valid and not null.");
			if (!hasProperConstructors (plugin))
                throw new PluginHasInvalidConstructorsException (
                    "Plugin DisplayUnit type needs to support the " +
                    "the constructor signatures of the DisplayUnit base class.");
            if (!plugin.DisplayUnitType.IsSubclassOf(typeof(DisplayUnit)))
                throw new NotDisplayUnitException (plugin.DisplayUnitType, "DisplayUnitType is not derived from DisplayUnit");
            try {
				_registry.Add (plugin.PluginId.Value, plugin);
            } catch (ArgumentException) {
                throw new PluginAlreadyRegisteredException ();
            }
        }

        public DisplayUnitPlugin Resolve(Guid PluginId)
        {
            DisplayUnitPlugin plugin;
            if(_registry.TryGetValue(PluginId, out plugin))
            {
                return plugin;
            }
            return null;
        }

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
            } catch (Exception ex) {
                passes = false;
            }
            return passes;


        }
    }
}

