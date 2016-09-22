using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates there was a problem having to do with dependencies, likely related to an action within
	/// an IContainer class.
    /// </summary>
	public class DependencyException : Exception
    {
        /// <summary>
        /// Gets or sets the type at issue.
        /// </summary>
        /// <value>The type at issue.</value>
		public Type TypeAtIssue {
            get;
            set;
        }

        protected DependencyException() : base()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.DependencyException"/> class.
		/// The dependency involved is passed in as a type.
		/// </summary>
		/// <param name="typeAtIssue">Type at issue.</param>
        public DependencyException (Type typeAtIssue) : base()
        {
            TypeAtIssue = typeAtIssue;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.DependencyException"/> class.
		/// The dependency involved is passed in as a type.
		/// <param name="typeAtIssue">Type at issue.</param>
		/// <param name="message">Message.</param>
        public DependencyException (Type typeAtIssue, string message) : base (message)
        {
            TypeAtIssue = typeAtIssue;
        }


        public DependencyException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.DependencyException"/> class.
		/// The dependency involved is passed in as a type.
		/// <param name="typeAtIssue">Type at issue.</param>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public DependencyException (Type typeAtIssue, string message, Exception innerException) : base (message, innerException)
        {
            TypeAtIssue = typeAtIssue;
        }

    }
}

