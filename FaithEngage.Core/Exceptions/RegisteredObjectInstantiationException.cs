using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates there was a problem constructing the ConcreteType of a registered object.
    /// </summary>
	public class RegisteredObjectInstantiationException : DependencyException
    {

        public RegisteredObjectInstantiationException ()
        {
        }
        

        public RegisteredObjectInstantiationException (Type typeAtIssue, string message) : base (typeAtIssue, message)
        {
        }
        

        public RegisteredObjectInstantiationException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

		public RegisteredObjectInstantiationException (Type typeAtIssue, string message, Exception innerException) : base (typeAtIssue, message, innerException)
        {
        }
        
    }
}

