using System;
namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Indicates that a plugin is missing information necessary to its functioning.
	/// </summary>
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

