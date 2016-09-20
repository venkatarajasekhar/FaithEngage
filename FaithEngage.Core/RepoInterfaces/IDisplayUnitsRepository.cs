using System;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.RepoInterfaces
{
    public interface IDisplayUnitsRepository
    {
        DisplayUnitDTO GetById(Guid id);
        DisplayUnitDTO PushDU (Guid id);
        DisplayUnitDTO PullDU (Guid id);
        Dictionary<int,DisplayUnitDTO> GetByEvent (Guid eventId, bool onlyPushed = false);
        Dictionary<int, DisplayUnitDTO> GetGroup (Guid eventId, Guid groupId);
        void SaveManyToEvent (Dictionary<int, DisplayUnitDTO> unitsAtPositions, Guid eventId);
        void SaveOneToEvent (DisplayUnitDTO unit);
        void Delete (Guid id);
    }
}

