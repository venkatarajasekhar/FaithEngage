using System;
using System.IO;
using FaithEngage.Core.Config;
using FaithEngage.Core.PluginManagers.Files.Interfaces;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
	public class PluginFileInfoFactory : IPluginFileInfoFactory
	{
		public PluginFileInfoFactory(IConfigManager config)
		{

		}
		public PluginFileInfo Convert(PluginFileInfoDTO source)
		{
			throw new NotImplementedException();
		}

		public PluginFileInfo Create(FileInfo file, Guid pluginId)
		{
			return new PluginFileInfo(pluginId, file);
		}
	}
}

