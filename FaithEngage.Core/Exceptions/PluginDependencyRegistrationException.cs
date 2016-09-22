using System;
namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates there was a problem with plugin dependencies. Generally, this is because a plugin's assembly references
	/// assemblies that are inaccessible.
    /// </summary>
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

