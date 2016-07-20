using System;
using System.IO.Compression;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.PluginManagers
{
	public class PluginManager : IPluginManager
	{
		public PluginManager(IPluginFileManager fileManager)
		{

		}

		public Guid Install(ZipArchive zipFile)
		{
			throw new NotImplementedException();
		}

		public void Uninstall(Guid pluginId)
		{
			throw new NotImplementedException();
		}
	}
}

