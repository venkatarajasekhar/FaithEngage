using System;
using System.Collections.Generic;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    public interface IDisplayUnitsRepoManager
    {
        DisplayUnit GetById (Guid unitId);
        Dictionary<int,DisplayUnit> GetByEvent (Guid eventId, bool onlyPushed = false);
        void SaveManyToEvent (Dictionary<int,DisplayUnit> unitsAtPositions, Guid serviceId);
        void SaveOneToEvent (DisplayUnit unit);
        void SaveDtoToEvent(DisplayUnitDTO dto);
        void DuplicateToEvent(DisplayUnit unit);
        void Delete (Guid unitId);
        DisplayUnit PushDU (Guid id);
        DisplayUnit PullDu (Guid id);
        Dictionary<int,DisplayUnit> GetGroup (Guid eventId, Guid groupId);
    }
}

