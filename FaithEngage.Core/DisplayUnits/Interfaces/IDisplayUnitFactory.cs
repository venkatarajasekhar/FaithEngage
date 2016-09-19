using System;
using System.Collections.Generic;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.DisplayUnits.Interfaces
{
	public interface IDisplayUnitFactory : IConverterFactory<DisplayUnitDTO, DisplayUnit>
    {
        DisplayUnit InstantiateNew (Guid PluginId, Dictionary<string,string> attributes);
    }
}

