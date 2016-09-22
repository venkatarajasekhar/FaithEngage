using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates there was a problem interacting with a file, likely due to an invalid file path. 
    /// </summary>
	public class InvalidFileException : Exception
    {
        /// <summary>
        /// The path to the file that is problematic.
        /// </summary>
        /// <value>The bad file reference.</value>
		public string BadFileReference {
            get;
            set;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidFileException"/> class.
		/// </summary>
		/// <param name="badFileReference">The path to the bad file reference</param>
        public InvalidFileException (string badFileReference) : base()
        {
            BadFileReference = badFileReference;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidFileException"/> class.
		/// </summary>
		/// <param name="badFileReference">The path to the bad file reference</param>
		/// <param name="message">Message.</param>
        public InvalidFileException (string badFileReference, string message) : base(message)
        {
            BadFileReference = badFileReference;
        }

        public InvalidFileException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidFileException"/> class.
		/// </summary>
		/// <param name="badFileReference">The path to the bad file reference</param>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public InvalidFileException (string badFileReference, string message, Exception innerException) : base(message,innerException)
        {
            BadFileReference = badFileReference;
        }
    }
}

