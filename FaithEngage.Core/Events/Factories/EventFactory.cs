using System;
using FaithEngage.Core.Events.EventSchedules.Interfaces;
using FaithEngage.Core.Events.Interfaces;

namespace FaithEngage.Core.Events.Factories
{
	public class EventFactory : IConverterFactory<EventDTO,Event>
	{
		private IEventScheduleRepoManager _schedRepoMgr;
		public EventFactory(IEventScheduleRepoManager schedRepoMgr)
		{
			_schedRepoMgr = schedRepoMgr;
		}
		public Event Convert(EventDTO dto)
		{
			throw new NotImplementedException();
		}
	}
}

