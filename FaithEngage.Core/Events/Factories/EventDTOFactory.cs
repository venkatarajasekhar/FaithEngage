using System;
using FaithEngage.Core.Events.Interfaces;

namespace FaithEngage.Core.Events.Factories
{
	public class EventDTOFactory : IConverterFactory<Event,EventDTO>
	{
		public EventDTO Convert(Event evnt)
		{
			throw new NotImplementedException();
		}
	}
}

