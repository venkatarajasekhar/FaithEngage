using System;
namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Indicates a problem was encountered in the process of uninstalling a plugin.
	/// </summary>
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
