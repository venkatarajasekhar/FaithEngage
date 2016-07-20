using System;
using System.IO.Compression;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
    public interface IPluginManager
    {
		Guid Install(ZipArchive zipFile);
		void Uninstall(Guid pluginId);
    }
}

