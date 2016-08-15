using System;
namespace FaithEngage.Core
{
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

