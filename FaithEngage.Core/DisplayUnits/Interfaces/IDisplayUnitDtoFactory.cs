using System;
namespace FaithEngage.Core.DisplayUnits.Interfaces
{
    public interface IDisplayUnitDtoFactory
    {
        DisplayUnitDTO ConvertToDto (DisplayUnit unit);
    }
}

