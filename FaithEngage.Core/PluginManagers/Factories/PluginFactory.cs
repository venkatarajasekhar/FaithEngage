using System;
using System.Reflection;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Factories
{
	public class PluginFactory : IConverterFactory<PluginDTO,Plugin>
	{
		

		public Plugin Convert(PluginDTO source)
		{
			var assembly = Assembly.LoadFrom(source.AssemblyLocation);
			//add null check
			var type = assembly.GetType(source.FullName);
			//add null check
			var ctor = type.GetConstructor(new Type[] { });
			//add null check
			var plugin = (Plugin) ctor.Invoke(new Object[] { });
            plugin.PluginId = source.Id;
			return plugin;
		}
	}
}

