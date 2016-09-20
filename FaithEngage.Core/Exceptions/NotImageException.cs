using System;

namespace FaithEngage.Core.Exceptions
{
    public class NotImageException : InvalidFileException
    {
        public NotImageException (string badFileReference) : base (badFileReference)
        {
        }

        public NotImageException (string badFileReference, string message) : base (badFileReference, message)
        {
        }

        public NotImageException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public NotImageException (string badFileReference, string message, Exception innerException) : base (badFileReference, message, innerException)
        {
        }
       
    }
}

