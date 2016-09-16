using System;
namespace FaithEngage.Core.Exceptions
{
    public class PluginInitializationException : Exception
    {
        public PluginInitializationException ()
        {
        }

        public PluginInitializationException (string message) : base (message)
        {
        }

        public PluginInitializationException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public PluginInitializationException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}

