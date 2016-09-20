using System;

namespace FaithEngage.Core
{
    public class InvalidAttributesException : Exception
    {
        public InvalidAttributesException ()
        {
        }
        

        public InvalidAttributesException (string message) : base (message)
        {
        }
        

        public InvalidAttributesException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public InvalidAttributesException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

