using System;
using System.IO;
namespace FaithEngage.Core.PluginManagers.Files
{
    /// <summary>
    /// Conatainer for information relating to a file for a plugin. 
    /// </summary>
	public class PluginFileInfo
    {
        private Guid _pluginId;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginManagers.Files.PluginFileInfo"/> class.
		/// A new file id is set for the file.
		/// </summary>
		/// <param name="pluginId">Plugin identifier.</param>
		/// <param name="fileInfo">File info.</param>
        public PluginFileInfo (Guid pluginId, FileInfo fileInfo)
        {
            _pluginId = pluginId;
			FileInfo = fileInfo;
			FileId = Guid.NewGuid();
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginManagers.Files.PluginFileInfo"/> class.
		/// The file's id is passed in and set.
		/// </summary>
		/// <param name="pluginId">Plugin identifier.</param>
		/// <param name="fileInfo">File info.</param>
		/// <param name="fileId">File identifier.</param>
		public PluginFileInfo(Guid pluginId, FileInfo fileInfo, Guid fileId)
		{
			FileId = fileId;
			_pluginId = pluginId;
			FileInfo = fileInfo;
		}
		/// <summary>
		/// Gets the plugin identifier.
		/// </summary>
		/// <value>The plugin identifier.</value>
        public Guid PluginId{ get { return _pluginId;}}
        /// <summary>
        /// Gets or sets the file identifier.
        /// </summary>
        /// <value>The file identifier.</value>
		public Guid FileId {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the file info for this file.
		/// </summary>
		/// <value>The file info.</value>
        public FileInfo FileInfo{ get; set; }
    }
}

