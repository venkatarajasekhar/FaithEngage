using System;

namespace FaithEngage.Core.Exceptions
{
    public class UnParsedReferenceObjectException : Exception
    {
        public UnParsedReferenceObjectException ()
        {
        }
        

        public UnParsedReferenceObjectException (string message) : base (message)
        {
        }
        

        public UnParsedReferenceObjectException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public UnParsedReferenceObjectException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

