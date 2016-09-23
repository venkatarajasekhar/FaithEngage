using System;
using System.IO;
using FaithEngage.Core.Config;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.PluginManagers.Files.Factories
{
    /// <summary>
    /// Converts and creates new PluginFileInfo objects.
    /// </summary>
	public class PluginFileInfoFactory : IPluginFileInfoFactory
    {

        private DirectoryInfo _tempFolder;
        private DirectoryInfo _pluginsFolder;

		/// <summary>
		/// Gets the temp folder's DirectoryInfo.
		/// </summary>
		/// <value>The temp folder.</value>
        public DirectoryInfo TempFolder { get { return _tempFolder; } }
        /// <summary>
        /// Gets the plugins folder's DirectoryInfo.
        /// </summary>
        /// <value>The plugins folder.</value>
		public DirectoryInfo PluginsFolder { get { return _pluginsFolder; } }
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:FaithEngage.Core.PluginManagers.Files.Factories.PluginFileInfoFactory"/> class.
		/// </summary>
		/// <remarks>If the temp and plugins folders do not exist, they will be created.</remarks>
		/// <param name="config">Config.</param>
		public PluginFileInfoFactory (IConfigManager config)
        {
            _tempFolder = new DirectoryInfo (getOSSafePath (config.TempFolderPath));
            if (!_tempFolder.Exists) _tempFolder.Create ();
            _pluginsFolder = new DirectoryInfo (getOSSafePath (config.PluginsFolderPath));
            if (!_pluginsFolder.Exists) _pluginsFolder.Create ();
        }
		/// <summary>
		/// Convert the specified PluginFileInfoDTO.
		/// </summary>
		/// <param name="source">Source.</param>
        public PluginFileInfo Convert (PluginFileInfoDTO source)
        {
            //If there are any empty properties, throw.
			if (!validateDto (source)) 
                throw new FactoryException (
                    "Dto has one or more empty properties. All properties required.");
            //Make sure the path is relavant to the current os's path structure
			var relPath = getOSSafePath (source.RelativePath);
			//Create the full path from the plugin path and the relative path.
            var fullPath = Path.Combine (GetBasePluginPath (source.PluginId), relPath);
            //Create a new fileinfo from the full path of the file
			var fInfo = new FileInfo (fullPath);
            //If it doesn't exist, return null
			if (!fInfo.Exists) return null;
            //Create a new PluginFileInfo with the fileinfo
			return new PluginFileInfo (source.PluginId, fInfo, source.FileId);
        }

		/// <summary>
		/// Validates the dto, ensuring all properties are populated.
		/// </summary>
		/// <returns><c>true</c>, if dto was validated, <c>false</c> otherwise.</returns>
		/// <param name="dto">Dto.</param>
        private bool validateDto (PluginFileInfoDTO dto)
        {
            if (dto.FileId == Guid.Empty) return false;
            if (String.IsNullOrWhiteSpace(dto.Name)) return false;
            if (dto.PluginId == Guid.Empty) return false;
            if (String.IsNullOrWhiteSpace(dto.RelativePath)) return false;
            return true;
        }

		/// <summary>
		/// Creates a new PluginFileInfo
		/// </summary>
		/// <param name="file">File.</param>
		/// <param name="pluginId">Plugin identifier.</param>
		public PluginFileInfo Create(FileInfo file, Guid pluginId)
		{
			return new PluginFileInfo(pluginId, file);
		}

		/// <summary>
		/// Gets the OS safe path with the correct directory separator ('/' in mac/linux, '\'
		/// in Windows).
		/// </summary>
		/// <returns>The OSS afe path.</returns>
		/// <param name="unsafePath">Unsafe path.</param>
		private string getOSSafePath(string unsafePath)
		{
			var segments = unsafePath.Split('/', '\\');
			return Path.Combine(segments);
		}
        /// <summary>
        /// Gets the base plugin path for the plugin with the specified id.
        /// </summary>
        /// <returns>The base plugin path.</returns>
        /// <param name="pluginId">Plugin identifier.</param>
		public string GetBasePluginPath(Guid pluginId)
		{
			return Path.Combine(_pluginsFolder.FullName, pluginId.ToString());
		}

		/// <summary>
		/// Renames a path of a PluginFileInfo to a new relative path (within its plugin directory).
		/// </summary>
		/// <returns>The renamed path.</returns>
		/// <param name="pluginFile">Plugin file.</param>
		/// <param name="newRelativePath">New relative path.</param>
		public string GetRenamedPath(PluginFileInfo pluginFile, string newRelativePath)
		{
			return Path.Combine(GetBasePluginPath(pluginFile.PluginId), getOSSafePath(newRelativePath));
		}
	}
}

