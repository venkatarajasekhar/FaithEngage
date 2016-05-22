using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Scripture reference parse exception.
    /// </summary>
    public class ScriptureReferenceParseException : Exception
    {
        /// <summary>
        /// Gets or sets the bad reference.
        /// </summary>
        /// <value>The bad reference.</value>
        public string BadReference {
            get;
            set;
        }


        public ScriptureReferenceParseException (string reference)
        {
            BadReference = reference;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FaithEngage.Core.ScriptureReferenceParseException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public ScriptureReferenceParseException (string message, string reference) : base (message)
        {
            BadReference = reference;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FaithEngage.Core.ScriptureReferenceParseException"/> class.
        /// </summary>
        /// <param name="info">Info.</param>
        /// <param name="context">Context.</param>
        public ScriptureReferenceParseException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FaithEngage.Core.ScriptureReferenceParseException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ScriptureReferenceParseException (string message, string reference, Exception innerException) : base (message, innerException)
        {
            BadReference = reference;
        }
        
    }
}

