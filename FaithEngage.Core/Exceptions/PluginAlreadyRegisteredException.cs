using System;

namespace FaithEngage.Core
{
    public class PluginAlreadyRegisteredException : Exception
    {
		public Guid ExistingPluginId { get; set; }
		public PluginAlreadyRegisteredException ()
        {
        }
        

        public PluginAlreadyRegisteredException (string message) : base (message)
        {
        }
        

        public PluginAlreadyRegisteredException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public PluginAlreadyRegisteredException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

