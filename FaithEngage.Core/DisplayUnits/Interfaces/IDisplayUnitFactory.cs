using System;
using System.Collections.Generic;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
	public interface IDisplayUnitFactory : IConverterFactory<DisplayUnitDTO, DisplayUnit>
    {
        DisplayUnit InstantiateNew (Guid PluginId, Dictionary<string,string> attributes);
    }
}

