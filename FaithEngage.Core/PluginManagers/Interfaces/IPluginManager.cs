using System;
using System.IO.Compression;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.Collections.Generic;
using System.IO;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
    public interface IPluginManager
    {
		int Install(ZipArchive zipFile);
        void Install<TPlugin> (IList<FileInfo> files = null) where TPlugin : Plugin, new();
		void Uninstall(Guid pluginId);
		void InitializeAllPlugins();
        bool CheckRegistered (Guid pluginId);
        bool CheckRegistered<TPlugin> () where TPlugin : Plugin;
    }
}

