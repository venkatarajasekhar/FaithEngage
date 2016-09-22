using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates a position in a grouping is being set with a negative number and negative positions are disallowed.
    /// </summary>
	public class NegativePositionException : Exception
    {
        public NegativePositionException ()
        {
        }
        

        public NegativePositionException (string message) : base (message)
        {
        }
        

        public NegativePositionException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public NegativePositionException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

