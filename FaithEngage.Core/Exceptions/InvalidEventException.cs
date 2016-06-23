using System;
using FaithEngage.Core.Events;

namespace FaithEngage.Core.Exceptions
{
	public class InvalidEventException : Exception
	{
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

