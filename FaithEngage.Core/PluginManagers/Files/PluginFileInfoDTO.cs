using System;
namespace FaithEngage.Core.PluginManagers.Files
{
    public class PluginFileInfoDTO
    {
        public string RelativePath { get; set;}
        public string Name { get; set;}
        public Guid FileId { get; set;}
        public Guid PluginId { get; set;}
    }
}

