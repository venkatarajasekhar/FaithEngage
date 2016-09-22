using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// A Plugin is being requested that is not registered and therefore its id is not recognized.
    /// </summary>
	public class NotRegisteredPluginException : Exception
    {
        public NotRegisteredPluginException ()
        {
        }
        

        public NotRegisteredPluginException (string message) : base (message)
        {
        }
        

        public NotRegisteredPluginException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public NotRegisteredPluginException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

