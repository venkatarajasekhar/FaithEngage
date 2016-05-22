using System;

namespace FaithEngage.Core.Exceptions
{
    public class EmptyGuidException : Exception
    {
        public EmptyGuidException ()
        {
        }
        

        public EmptyGuidException (string message) : base (message)
        {
        }
        

        public EmptyGuidException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public EmptyGuidException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

