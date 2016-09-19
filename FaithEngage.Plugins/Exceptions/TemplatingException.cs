using System;
namespace FaithEngage.CorePlugins.Exceptions
{
    public class TemplatingException : Exception
    {
        public TemplatingException ()
        {
        }

        public TemplatingException (string message) : base (message)
        {
        }

        public TemplatingException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public TemplatingException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}

