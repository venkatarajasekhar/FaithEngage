using System;

namespace FaithEngage.Core.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException ()
        {
        }
        

        public InvalidUsernameException (string message) : base (message)
        {
        }
        

        public InvalidUsernameException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public InvalidUsernameException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

