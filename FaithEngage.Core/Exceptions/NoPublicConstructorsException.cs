using System;
namespace FaithEngage.Core
{
    public class NoPublicConstructorsException : Exception
    {

        public NoPublicConstructorsException ()
        {
        }

        public NoPublicConstructorsException (string message) : base (message)
        {
        }

        public NoPublicConstructorsException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public NoPublicConstructorsException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}

