using System;

namespace FaithEngage.Core.Exceptions
{
    public class DependencyException : Exception
    {
        public Type TypeAtIssue {
            get;
            set;
        }

        protected DependencyException() : base()
        {
        }

        public DependencyException (Type typeAtIssue) : base()
        {
            TypeAtIssue = typeAtIssue;
        }


        public DependencyException (Type typeAtIssue, string message) : base (message)
        {
            TypeAtIssue = typeAtIssue;
        }


        public DependencyException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }


        public DependencyException (Type typeAtIssue, string message, Exception innerException) : base (message, innerException)
        {
            TypeAtIssue = typeAtIssue;
        }

    }
}

