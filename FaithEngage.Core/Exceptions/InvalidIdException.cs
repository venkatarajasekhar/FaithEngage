using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Thrown by a repository when a critical id is not valid and a null return value would be misleading.
    /// </summary>
	public class InvalidIdException : RepositoryException
    {
        public InvalidIdException ()
        {
        }
        

        public InvalidIdException (string message) : base (message)
        {
        }
        

        public InvalidIdException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public InvalidIdException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

