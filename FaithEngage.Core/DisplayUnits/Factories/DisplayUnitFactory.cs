using System;
using System.Collections.Generic;
using System.Reflection;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.DisplayUnits.Factories
{
    public class DisplayUnitFactory : IDisplayUnitFactory
    {
        private readonly IDisplayUnitPluginContainer _container;
        public DisplayUnitFactory (IDisplayUnitPluginContainer plginCtr)
        {
            _container = plginCtr;
        }
        public DisplayUnit InstantiateNew (Guid pluginId, Dictionary<string, string> attributes)
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

        private DisplayUnit getDisplayUnit(Guid pluginId, Guid Id, Dictionary<string,string> attributes)
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

        private DisplayUnit getDisplayUnit(Guid pluginId, Dictionary<string,string> attributes)
        {
            //Get the constructor that accepts only a dictionary<string,string>.
			var ctor = getCtor (pluginId, new Type[]{typeof(Dictionary<string,string>)});
            //Invoke it with the attributes dictionary.
			var unit = ctor.Invoke (new object[]{ attributes }) as DisplayUnit;
            return unit;
        }

        private ConstructorInfo getCtor(Guid pluginId, Type[] paramTypes)
        {
            var plugin = _container.Resolve (pluginId);
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
			unit.Plugin.PluginId = dto.PluginId;
            if(dto.GroupId.HasValue && dto.PositionInGroup.HasValue){
                unit.UnitGroup = new DisplayUnitGrouping (dto.PositionInGroup.Value, dto.GroupId.Value);
            }
            return unit;
        }


    }
}

