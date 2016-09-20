using System;

namespace FaithEngage.Core.Exceptions
{
    public class CouldNotFindException : RepositoryException
    {
        public CouldNotFindException ()
        {
        }
        

        public CouldNotFindException (string message) : base (message)
        {
        }
        

        public CouldNotFindException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public CouldNotFindException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

