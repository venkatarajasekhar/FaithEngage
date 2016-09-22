using System;
namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates a problem with initializing a plugin. This is a catch-all exception for all exceptions encountered
	/// during the initialization process for a given plugin.
    /// </summary>
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

