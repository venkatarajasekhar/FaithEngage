using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.PluginManagers.Factories
{
	public class PluginDtoFactory : IConverterFactory<Plugin,PluginDTO>
    {
        public PluginDTO Convert(Plugin plugin)
        {
			validate(plugin);
			var dto = new PluginDTO ();
			dto.Id = plugin.PluginId;
			dto.AssemblyLocation = plugin.AssemblyLocation;
			dto.FullName = plugin.FullName;
			dto.PluginName = plugin.PluginName;
			dto.PluginVersion = plugin.PluginVersion;
			dto.PluginType = plugin.PluginType;
            return dto;
        }

		void validate(Plugin plugin)
		{
			if(plugin.AssemblyLocation == "") throwInvalidException("AssemblyLocation");
			if(plugin.AssemblyLocation == null) throwInvalidException("AssemblyLocation");
			if(plugin.FullName == "") throwInvalidException("FullName");
			if(plugin.FullName == null) throwInvalidException("FullName");
			if(plugin.PluginName == "") throwInvalidException("PluginName");
			if(plugin.PluginName == null) throwInvalidException("PluginName");
			if(plugin.PluginVersion == null) throwInvalidException("PluginVersion");
		}

		void throwInvalidException(string invalidProp)
		{
			throw new PluginIsMissingNecessaryInfoException("The Plugin had invalid info that was necessary: " + invalidProp);
		}
	}
}

