using System;

namespace FaithEngage.Core.Exceptions
{
    public class CouldNotConvertDTOException : Exception
    {
        public CouldNotConvertDTOException ()
        {
        }
        

        public CouldNotConvertDTOException (string message) : base (message)
        {
        }
        

        public CouldNotConvertDTOException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public CouldNotConvertDTOException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

