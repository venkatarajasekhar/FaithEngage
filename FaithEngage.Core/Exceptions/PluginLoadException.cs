﻿using System;
namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates there was a problem loading a plugin's assembly and registering it.
    /// </summary>
	public class PluginLoadException : Exception
    {
        public PluginLoadException ()
        {
        }

        public PluginLoadException (string message) : base (message)
        {
        }

        public PluginLoadException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public PluginLoadException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}

