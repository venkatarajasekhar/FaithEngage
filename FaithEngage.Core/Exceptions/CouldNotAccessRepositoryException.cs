using System;

namespace FaithEngage.Core.Exceptions
{
    public class CouldNotAccessRepositoryException : RepositoryException
    {
        public CouldNotAccessRepositoryException ()
        {
        }
        

        public CouldNotAccessRepositoryException (string message) : base (message)
        {
        }
        

        public CouldNotAccessRepositoryException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public CouldNotAccessRepositoryException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

