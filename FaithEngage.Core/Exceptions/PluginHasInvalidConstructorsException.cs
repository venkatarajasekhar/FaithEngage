using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates the plugin desired cannot be constructed because it's constructor signature doesn't match the
	/// required signature type.
    /// </summary>
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

