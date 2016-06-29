using System;
namespace FaithEngage.Core.Exceptions
{
	public class InvalidTimeZoneException : Exception
	{
		public InvalidTimeZoneException()
		{
		}

		public InvalidTimeZoneException(string message) : base(message)
		{
		}

		public InvalidTimeZoneException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public InvalidTimeZoneException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}

