using System;

namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Indicates there was a failure converting from one type to a data transfer object within a factory.
	/// </summary>
	public class CouldNotConvertDTOException : FactoryException
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

