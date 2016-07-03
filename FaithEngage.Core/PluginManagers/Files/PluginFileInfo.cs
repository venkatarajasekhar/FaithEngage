using System;
using System.IO;
namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileInfo
    {
        private Guid _pluginId;
        private FileInfo _fileInfo;

        public PluginFileInfo (Guid pluginId, FileInfo fileInfo)
        {
            _pluginId = pluginId;
            _fileInfo = fileInfo;
        }

        public Guid PluginId{ get { return _pluginId;}}
        public Guid FileId {
            get;
            set;
        }

        public FileInfo FileInfo{ get { return _fileInfo;}}
    }
}

