using System;
using System.Collections.Generic;
using System.Reflection;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.DisplayUnits.Factories
{
    /// <summary>
    /// Creates new DisplayUnits and converts DisplayUnitDTOs
    /// </summary>
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

        public DisplayUnit Convert (DisplayUnitDTO dto)
        {
            try {
                DisplayUnit unit;
				//If the dto has no id (i.e. it was created from the front end and hasn't been saved yet)...
				if(dto.Id == Guid.Empty){
                    //Construct the display unit
					unit = getDisplayUnit (dto.PluginId, dto.Attributes);
                }
                else
                {
                    unit = getDisplayUnit (dto.PluginId, dto.Id, dto.Attributes);
                }
				//Apply the dtos properties to the constructed display unit.
                unit = applyDtoProperties (dto, unit);
                return unit;
            } catch (Exception) {//If an exception, return null.
                return null;
            }
        }

		/// <summary>
		/// Obtains a constructed display unit with the specified plugin, id, and attributes.
		/// </summary>
		/// <returns>The display unit.</returns>
		/// <param name="pluginId">The Plugin Id</param>
		/// <param name="Id">The display unit's id</param>
		/// <param name="attributes">The attributes dictionary</param>
        private DisplayUnit getDisplayUnit(Guid pluginId, Guid Id, Dictionary<string,string> attributes)
        {
            //Create a type array the DisplayUnit's constructor MUST have.
            var types = new Type[]
                {
                    typeof(Guid),
                    typeof(Dictionary<string,string>)
                };
            //Get the constructor for display unit with the given plugin type and constructor types specified
			var ctor = getCtor (pluginId, types);
            //Invoke the constructor with the id and attributes
			var unit = ctor.Invoke (new object[]{ Id, attributes }) as DisplayUnit;
            //Return the display unit, now constructed.
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

		/// <summary>
		/// Obtains the constructor for the given plugin identified by its id.
		/// </summary>
		/// <returns>The constructor</returns>
		/// <param name="pluginId">Plugin identifier.</param>
		/// <param name="paramTypes">Constructor paramater types</param>
        private ConstructorInfo getCtor(Guid pluginId, Type[] paramTypes)
        {
            //Get the plugin from the container.
			var plugin = _container.Resolve (pluginId);
            if (plugin == null)
                throw new NotRegisteredPluginException ("Plugin not registered: " + pluginId);
            //Get the display unit type from the plugin.
			var type = plugin.DisplayUnitType;
            //Get the constructor of the display unit type with the specific parameter types
			return type.GetConstructor (paramTypes);
        }
		/// <summary>
		/// Applies the dto properties to the new Display Unit.
		/// </summary>
		/// <returns>The dto properties.</returns>
		/// <param name="dto">Dto.</param>
		/// <param name="unit">Unit.</param>
        private DisplayUnit applyDtoProperties(DisplayUnitDTO dto, DisplayUnit unit)
        {
            unit.Name = dto.Name;
            unit.Description = dto.Description;
            unit.DateCreated = dto.DateCreated;
            unit.AssociatedEvent = dto.AssociatedEventId;
            unit.PositionInEvent = dto.PositionInEvent;
			unit.Plugin.PluginId = dto.PluginId;
            if(dto.GroupId.HasValue && dto.PositionInGroup.HasValue){
                unit.UnitGroup = new DisplayUnitGrouping (dto.PositionInGroup.Value, dto.GroupId.Value);
            }
            return unit;
        }


    }
}

