using System;
using System.Collections.Generic;
using System.Reflection;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.DisplayUnits
{
    public class DisplayUnitFactory : IDisplayUnitFactory
    {
        private readonly IDisplayUnitPluginContainer _mgr;
        public DisplayUnitFactory (IDisplayUnitPluginContainer plginCtr)
        {
            _mgr = plginCtr;
        }
        public DisplayUnit InstantiateNew (string pluginId, Dictionary<string, string> attributes)
        {
            return getDisplayUnit (pluginId, attributes);
        }

        public DisplayUnit ConvertFromDto (DisplayUnitDTO dto)
        {
            try {
                DisplayUnit unit;
                if(dto.Id == Guid.Empty){
                    unit = getDisplayUnit (dto.PluginId, dto.Attributes);
                }
                else
                {
                    unit = getDisplayUnit (dto.PluginId, dto.Id, dto.Attributes);
                }

                unit = applyDtoProperties (dto, unit);
                return unit;
            } catch (Exception) {
                return null;
            }
        }

        private DisplayUnit getDisplayUnit(string pluginId, Guid Id, Dictionary<string,string> attributes)
        {
            
            var types = new Type[]
                {
                    typeof(Guid),
                    typeof(Dictionary<string,string>)
                };
            var ctor = getCtor (pluginId, types);
            var unit = ctor.Invoke (new object[]{ Id, attributes }) as DisplayUnit;
            return unit;
        }

        private DisplayUnit getDisplayUnit(string pluginId, Dictionary<string,string> attributes)
        {
            var ctor = getCtor (pluginId, new Type[]{typeof(Dictionary<string,string>)});
            var unit = ctor.Invoke (new object[]{ attributes }) as DisplayUnit;
            return unit;
        }

        private ConstructorInfo getCtor(string pluginId, Type[] paramTypes)
        {
            var plugin = _mgr.Resolve (pluginId);
            if (plugin == null)
                throw new NotRegisteredPluginException ("Plugin not registered: " + pluginId);
            var type = plugin.DisplayUnitType;
            return type.GetConstructor (paramTypes);
        }

        private DisplayUnit applyDtoProperties(DisplayUnitDTO dto, DisplayUnit unit)
        {
            unit.Name = dto.Name;
            unit.Description = dto.Description;
            unit.DateCreated = dto.DateCreated;
            unit.AssociatedEvent = dto.AssociatedEvent;
            unit.PositionInEvent = dto.PositionInEvent;
            unit.UnitGroup = new DisplayUnitGrouping (dto.PositionInGroup, dto.GroupId);
            return unit;
        }


    }
}

