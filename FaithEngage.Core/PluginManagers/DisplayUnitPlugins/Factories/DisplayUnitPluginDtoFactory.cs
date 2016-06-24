using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
	public class DisplayUnitPluginDtoFactory : IConverterFactory<DisplayUnitPlugin,DisplayUnitPluginDTO>
    {
        public DisplayUnitPluginDTO Convert(DisplayUnitPlugin plugin)
        {
			validate(plugin);
			var dto = new DisplayUnitPluginDTO ();
			dto.Id = plugin.PluginId;
			dto.AssemblyLocation = plugin.AssemblyLocation;
			dto.FullName = plugin.FullName;
			dto.PluginName = plugin.PluginName;
			dto.PluginVersion = plugin.PluginVersion;
            return dto;
        }

		void validate(DisplayUnitPlugin plugin)
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

