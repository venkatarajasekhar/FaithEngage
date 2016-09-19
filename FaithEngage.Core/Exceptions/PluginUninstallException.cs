using System;
namespace FaithEngage.Core.Exceptions
{
	public class PluginUninstallException : Exception
	{
		public PluginUninstallException()
		{
		}

		public PluginUninstallException(string message) : base(message)
		{
		}

		public PluginUninstallException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public PluginUninstallException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}
