using System;
using System.IO.Compression;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
    public interface IPluginManager
    {
		int Install(ZipArchive zipFile);
		void Uninstall(Guid pluginId);
		void InitializeAllPlugins();
    }
}

