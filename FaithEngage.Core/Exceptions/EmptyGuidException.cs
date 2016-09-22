using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
	/// Indicates an id was necessary and an empty GUID (i.e. uninitialized) was used.
    /// </summary>
	public class EmptyGuidException : Exception
    {
        public EmptyGuidException ()
        {
        }
        

        public EmptyGuidException (string message) : base (message)
        {
        }
        

        public EmptyGuidException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public EmptyGuidException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

