using System;

namespace FaithEngage.Core.Exceptions
{
    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException ()
        {
        }
        

        public InvalidFileTypeException (string message) : base (message)
        {
        }
        

        public InvalidFileTypeException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public InvalidFileTypeException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

