using System;

namespace FaithEngage.Core.Exceptions
{
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

