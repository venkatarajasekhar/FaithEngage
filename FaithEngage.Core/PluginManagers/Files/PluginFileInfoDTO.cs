using System;
namespace FaithEngage.Core.PluginManagers.Files
{
    /// <summary>
    /// A data transfer object used for storing information related to a PluginFileInfo into the
	/// repository.
    /// </summary>
	public class PluginFileInfoDTO
    {
        public string RelativePath { get; set;}
        public string Name { get; set;}
        public Guid FileId { get; set;}
        public Guid PluginId { get; set;}
    }
}

