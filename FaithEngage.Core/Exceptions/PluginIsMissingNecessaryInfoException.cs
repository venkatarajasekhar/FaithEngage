using System;
namespace FaithEngage.Core.Exceptions
{
	public class PluginIsMissingNecessaryInfoException : Exception
	{
		public PluginIsMissingNecessaryInfoException()
		{
		}

		public PluginIsMissingNecessaryInfoException(string message) : base(message)
		{
		}

		public PluginIsMissingNecessaryInfoException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public PluginIsMissingNecessaryInfoException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}

