using System;
using FaithEngage.Core.Events;

namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Indicates an event was invalid--likely due to illogical or invalid data being assigned to it.
	/// </summary>
	public class InvalidEventException : Exception
	{
		/// <summary>
		/// Gets or sets the invalid event.
		/// </summary>
		/// <value>The invalid event.</value>
		public Event InvalidEvent
		{
			get;
			set;
		}
		public InvalidEventException()
		{
		}

		public InvalidEventException(string message) : base(message)
		{
		}

		public InvalidEventException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public InvalidEventException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}

