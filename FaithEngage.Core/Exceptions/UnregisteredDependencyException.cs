using System;

namespace FaithEngage.Core.Exceptions
{
    public class UnregisteredDependencyException : DependencyException
    {
        public UnregisteredDependencyException (Type typeAtIssue) : base (typeAtIssue)
        {
        }
        

        public UnregisteredDependencyException (Type typeAtIssue, string message) : base (typeAtIssue, message)
        {
        }
        

        public UnregisteredDependencyException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public UnregisteredDependencyException (Type typeAtIssue, string message, Exception innerException) : base (typeAtIssue, message, innerException)
        {
        }
        
        
    }
}

