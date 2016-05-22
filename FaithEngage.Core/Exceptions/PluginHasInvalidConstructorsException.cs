using System;

namespace FaithEngage.Core.Exceptions
{
    public class PluginHasInvalidConstructorsException : Exception
    {
        public PluginHasInvalidConstructorsException ()
        {
        }
        

        public PluginHasInvalidConstructorsException (string message) : base (message)
        {
        }
        

        public PluginHasInvalidConstructorsException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public PluginHasInvalidConstructorsException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

