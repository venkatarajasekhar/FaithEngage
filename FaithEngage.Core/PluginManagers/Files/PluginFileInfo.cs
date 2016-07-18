using System;
using System.IO;
namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileInfo
    {
        private Guid _pluginId;

        public PluginFileInfo (Guid pluginId, FileInfo fileInfo)
        {
            _pluginId = pluginId;
			FileInfo = fileInfo;
			FileId = Guid.NewGuid();
        }

		public PluginFileInfo(Guid pluginId, FileInfo fileInfo, Guid fileId)
		{
			FileId = fileId;
			_pluginId = pluginId;
			FileInfo = fileInfo;
		}

        public Guid PluginId{ get { return _pluginId;}}
        public Guid FileId {
            get;
            set;
        }

        public FileInfo FileInfo{ get; set; }
    }
}

