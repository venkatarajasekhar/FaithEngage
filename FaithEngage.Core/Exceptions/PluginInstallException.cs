using System;
namespace FaithEngage.Core
{
    /// <summary>
    /// Indicates a problem within the Plugin install process.
    /// </summary>
	public class PluginInstallException : Exception
    {
        public PluginInstallException ()
        {
        }

        public PluginInstallException (string message) : base (message)
        {
        }

        public PluginInstallException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public PluginInstallException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}

