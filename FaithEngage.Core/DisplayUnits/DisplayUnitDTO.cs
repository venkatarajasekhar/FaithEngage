using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.DisplayUnits
{
    /// <summary>
    /// This is the data transfer object for Display Units that allow for display units to be
	/// transfered cleanly between application layers.
    /// </summary>
	public class DisplayUnitDTO
    {
        public DisplayUnitDTO (Guid eventId, Guid? displayUnitId = null)
        {
			//Disallow empty guids for ids
			if (displayUnitId.Equals (Guid.Empty))				
				throw new EmptyGuidException ("Display unit id was not a valid id");
            //If the display unit id has a value, set it.
			if (displayUnitId.HasValue && displayUnitId.Value != Guid.Empty){
                Id = displayUnitId.Value;
            }else{//Else if there's no id specified, make a new one.
                Id = Guid.NewGuid();
            }
            //Disallow empty event ids
			if (eventId.Equals (Guid.Empty))
                throw new EmptyGuidException ("Event id was not a valid id");
            AssociatedEventId = eventId;
            Attributes = new Dictionary<string,string> ();
        }
		/// <summary>
		/// Gets or sets the DisplayUnit id.
		/// </summary>
		/// <value>The identifier.</value>
        public Guid Id {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>The date created.</value>
        public DateTime DateCreated {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
        public string Description {
            get;
            set;
        }
		//Todo: Associate DTOs with their du plugin by type fullname rather than id
        /// <summary>
        /// Gets or sets the plugin identifier.
        /// </summary>
        /// <value>The plugin identifier.</value>
		public Guid PluginId {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the attributes.
		/// </summary>
		/// <value>The attributes.</value>
        public Dictionary<string,string> Attributes {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the associated event id
		/// </summary>
		/// <value>The associated event.</value>
        public Guid AssociatedEventId {
            get;
            set;
        }

        private int _positionInEvent;
        /// <summary>
        /// Gets or sets the position in the event.
        /// </summary>
        /// <value>The position in event.</value>
		public int PositionInEvent {
            get{
                return _positionInEvent;
            }
            set{
                if(value < 0){
                    throw new NegativePositionException("Position of DisplayUnit must be positive.");
                }
                _positionInEvent = value;
            }
        }
		/// <summary>
		/// Gets or sets the position in the group.
		/// </summary>
		/// <value>The position in group.</value>
        public int? PositionInGroup{get;set;}
        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>The group identifier.</value>
		public Guid? GroupId{get;set;}
    }
}

