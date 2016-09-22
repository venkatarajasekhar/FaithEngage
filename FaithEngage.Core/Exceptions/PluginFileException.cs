using System;
namespace FaithEngage.Core
{
	/// <summary>
	/// Indicates there was a problem accessing a plugin file resource.
	/// </summary>
	public class PluginFileException : Exception
	{
		public PluginFileException()
		{
		}

		public PluginFileException(string message) : base(message)
		{
		}

		public PluginFileException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public PluginFileException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}

