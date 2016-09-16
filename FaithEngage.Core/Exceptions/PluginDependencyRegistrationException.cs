using System;
namespace FaithEngage.Core.Exceptions
{
    public class PluginDependencyRegistrationException : Exception
    {
        public PluginDependencyRegistrationException ()
        {
        }

        public PluginDependencyRegistrationException (string message) : base (message)
        {
        }

        public PluginDependencyRegistrationException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public PluginDependencyRegistrationException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}

