using System;

namespace FaithEngage.Core.Exceptions
{
    public class RegisteredObjectInstantiationException : Exception
    {

        public RegisteredObjectInstantiationException ()
        {
        }
        

        public RegisteredObjectInstantiationException (string message) : base (message)
        {
        }
        

        public RegisteredObjectInstantiationException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public RegisteredObjectInstantiationException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

