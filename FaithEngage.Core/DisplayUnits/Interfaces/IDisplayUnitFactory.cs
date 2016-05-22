using System;
using System.Collections.Generic;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    public interface IDisplayUnitFactory
    {
        DisplayUnit InstantiateNew (string PluginId, Dictionary<string,string> attributes);
        DisplayUnit ConvertFromDto(DisplayUnitDTO dto);
    }
}

