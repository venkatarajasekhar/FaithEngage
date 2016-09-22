using System;

namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Indicates the plugin requested has an invalid id.
	/// </summary>
	public class PluginHasInvalidIdException : Exception
	{
		public PluginHasInvalidIdException ()
		{
		}
		

		public PluginHasInvalidIdException (string message) : base (message)
		{
		}
		

		public PluginHasInvalidIdException (string message, Exception innerException) : base (message, innerException)
		{
		}
		

		public PluginHasInvalidIdException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
		
	}
}

