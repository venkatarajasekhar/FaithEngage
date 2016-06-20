using System;
using FaithEngage.Core.DisplayUnits.Interfaces;
namespace FaithEngage.Core.DisplayUnits.Factories
{
    public class DisplayUnitDtoFactory : IDisplayUnitDtoFactory
    {
        public DisplayUnitDTO ConvertToDto (DisplayUnit unit)
        {
			try
			{
				var dto = new DisplayUnitDTO(unit.AssociatedEvent, unit.Id);
				dto.Name = unit.Name;
				dto.DateCreated = unit.DateCreated;
				dto.Description = unit.Description;
				dto.PluginId = unit.Plugin.PluginId.Value;
				dto.PositionInEvent = unit.PositionInEvent;
				dto.Attributes = unit.GetAttributes();
				if (unit.UnitGroup.HasValue)
				{
					dto.PositionInGroup = unit.UnitGroup.Value.Position;
					dto.GroupId = unit.UnitGroup.Value.Id;
				}
				return dto;
			}
			catch
			{
				return null;
			}
        }
    }
}

