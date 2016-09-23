using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Factories
{
	/// <summary>
	/// Converts Plugins to PluginDTOs
	/// </summary>
	public class PluginDtoFactory : IConverterFactory<Plugin,PluginDTO>
    {
        /// <summary>
		/// Converts Plugins to PluginDTOs
		/// </summary>
		public PluginDTO Convert(Plugin plugin)
        {
			//Validate the plugin first.
			validate (plugin);
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
			if(string.IsNullOrWhiteSpace(plugin.FullName)) throwInvalidException("FullName");
			if(string.IsNullOrWhiteSpace(plugin.PluginName)) throwInvalidException("PluginName");
			if(plugin.PluginVersion == null) throwInvalidException("PluginVersion");
		}

		void throwInvalidException(string invalidProp)
		{
			throw new PluginIsMissingNecessaryInfoException("The Plugin had invalid info that was necessary: " + invalidProp);
		}
	}
}

