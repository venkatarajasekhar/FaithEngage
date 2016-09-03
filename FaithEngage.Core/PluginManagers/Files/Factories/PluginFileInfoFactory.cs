using System;
using System.IO;
using FaithEngage.Core.Config;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
    public class PluginFileInfoFactory : IPluginFileInfoFactory
    {

        private DirectoryInfo _tempFolder;
        private DirectoryInfo _pluginsFolder;

        public DirectoryInfo TempFolder { get { return _tempFolder; } }
        public DirectoryInfo PluginsFolder { get { return _pluginsFolder; } }

        public PluginFileInfoFactory (IConfigManager config)
        {
            _tempFolder = new DirectoryInfo (getOSSafePath (config.TempFolderPath));
            if (!_tempFolder.Exists) _tempFolder.Create ();
            _pluginsFolder = new DirectoryInfo (getOSSafePath (config.PluginsFolderPath));
            if (!_pluginsFolder.Exists) _pluginsFolder.Create ();
        }

        public PluginFileInfo Convert (PluginFileInfoDTO source)
        {
            if (!validateDto (source)) 
                throw new FactoryException (
                    "Dto has one or more empty properties. All properties required.");
            var relPath = getOSSafePath (source.RelativePath);
            var fullPath = Path.Combine (GetBasePluginPath (source.PluginId), relPath);
            var fInfo = new FileInfo (fullPath);
            if (!fInfo.Exists) return null;
            return new PluginFileInfo (source.PluginId, fInfo, source.FileId);
        }

        private bool validateDto (PluginFileInfoDTO dto)
        {
            if (dto.FileId == Guid.Empty) return false;
            if (String.IsNullOrWhiteSpace(dto.Name)) return false;
            if (dto.PluginId == Guid.Empty) return false;
            if (String.IsNullOrWhiteSpace(dto.RelativePath)) return false;
            return true;
        }

		public PluginFileInfo Create(FileInfo file, Guid pluginId)
		{
			return new PluginFileInfo(pluginId, file);
		}

		private string getOSSafePath(string unsafePath)
		{
			var segments = unsafePath.Split('/', '\\');
			return Path.Combine(segments);
		}
        public string GetBasePluginPath(Guid pluginId)
		{
			return Path.Combine(_pluginsFolder.FullName, pluginId.ToString());
		}

		public string GetRenamedPath(PluginFileInfo pluginFile, string newRelativePath)
		{
			return Path.Combine(GetBasePluginPath(pluginFile.PluginId), getOSSafePath(newRelativePath));
		}
	}
}

