using System;

namespace FaithEngage.Core.Exceptions
{
    public class InvalidFileException : Exception
    {
        public string BadFileReference {
            get;
            set;
        }

        public InvalidFileException (string badFileReference) : base()
        {
            BadFileReference = badFileReference;
        }

        public InvalidFileException (string badFileReference, string message) : base(message)
        {
            BadFileReference = badFileReference;
        }

        public InvalidFileException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

        public InvalidFileException (string badFileReference, string message, Exception innerException) : base(message,innerException)
        {
            BadFileReference = badFileReference;
        }
    }
}

