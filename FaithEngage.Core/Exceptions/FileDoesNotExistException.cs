using System;

namespace FaithEngage.Core.Exceptions
{
    public class FileDoesNotExistException : InvalidFileException
    {
        public FileDoesNotExistException (string badFileReference) : base (badFileReference)
        {
        }
        

        public FileDoesNotExistException (string badFileReference, string message) : base (badFileReference, message)
        {
        }
        

        public FileDoesNotExistException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public FileDoesNotExistException (string badFileReference, string message, Exception innerException) : base (badFileReference, message, innerException)
        {
        }
        
    }
}

