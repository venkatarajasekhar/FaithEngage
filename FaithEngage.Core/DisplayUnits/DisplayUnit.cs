using System;
using System.Collections.Generic;
using FaithEngage.Core.Cards;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files;


namespace FaithEngage.Core.DisplayUnits
{
    /// <summary>
    /// This is the base display unit from which all other units are derived. A DisplayUnit is an object
	/// with information about itself and with methods that allow it to produce a renderable card that
	/// will ultimately produce the rendered front end visual for a given DisplayUnit
    /// </summary>
    abstract public class DisplayUnit
    {
            
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractClasses.DisplayUnit"/> class, 
		/// assigning a new id.
        /// </summary>
		protected DisplayUnit (Dictionary<string,string> attributes) : this(Guid.NewGuid(),attributes)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.DisplayUnits.DisplayUnit"/> class
		/// with a preexisting id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="attributes">Attributes.</param>
        protected DisplayUnit(Guid id, Dictionary<string,string> attributes)
        {
            //Fail on empty Guid
			if (id == Guid.Empty)
                throw new EmptyGuidException ();
            //Assign the id to the DisplayUnit
			this.Id = id;
            //Attempt 
			tryApplyAttributes (attributes);
            SetAttributes (attributes);
        }

		/// <summary>
		/// Apply the attributes for this base class's properties.
		/// </summary>
		/// <param name="attributes">Attributes.</param>
        private void tryApplyAttributes(Dictionary<string,string> attributes)
        {
            string name;
            attributes.TryGetValue("Name", out name);
            Name = name;

            string description;
            attributes.TryGetValue("Description", out description);
            Description = description;

            string dateTime;
            attributes.TryGetValue ("DateCreated", out dateTime);
            DateTime date;
            DateTime.TryParse (dateTime,out date);
            DateCreated = date;

            string guid;
            attributes.TryGetValue ("AssociatedEvent", out guid);
            Guid id;
            Guid.TryParse (guid, out id);
            AssociatedEvent = id;

            string group;
            attributes.TryGetValue ("GroupId", out group);
            Guid groupId;
            var groupSet = Guid.TryParse (group, out groupId);

            string posInGroup;
            attributes.TryGetValue ("PositionInGroup", out posInGroup);
            int intPos;
            var posSet = int.TryParse (posInGroup, out intPos);

            if (posSet && groupSet)
                UnitGroup = new DisplayUnitGrouping (intPos, groupId);

            string pos;
            attributes.TryGetValue ("PositionInEvent", out pos);
            int position;
            int.TryParse (pos, out position);
            PositionInEvent = position;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id {
            get;
            private set;
        }

        /// <summary>
        /// Each display unit type is responsible for mapping and converting its properties into
        /// a Dictionary<string,string> to be passed out for storage and useage.
        /// </summary>
        /// <returns>The attributes.</returns>
        abstract public Dictionary<string,string> GetAttributes ();

		/// <summary>
		/// Obtains an attribute specified by the given key.
		/// </summary>
		/// <returns>The attribute.</returns>
		/// <param name="key">Key.</param>
        public string GetAttribute(string key)
        {
            var dict = this.GetAttributes ();
            string val;
            dict.TryGetValue (key, out val);
            return val;
        }

        /// <summary>
        /// Each display unit type is responsible for providing its accompanying plugin.
        /// </summary>
        /// <value>The plugin.</value>
        abstract public DisplayUnitPlugin Plugin { get;}

        /// <summary>
        /// Gets an IRenderableCard that will be rendered.
        /// </summary>
        /// <returns>The cards.</returns>
        abstract public IRenderableCard GetCard (ITemplatingService service, IDictionary<Guid, PluginFileInfo> files);

        /// <summary>
        /// If a display unit type has actions that should be performed as a response to
        /// interaction with a renderable card (i.e. external service calls, etc...),
        /// it will be sent to this method. These calls will be made via a response API.
		/// (TBD)
        /// </summary>
        /// <param name="Action">Action.</param>
        public virtual void ExecuteCardAction (CardAction Action)
        {
        }

		/// <summary>
		/// Any card action executed, if a response is to be sent back, is done via this event.
		/// </summary>
        public virtual event CardActionResultEventHandler OnCardActionResult;

        /// <summary>
        /// If this Display Unit is part of a larger group (such as a series of slides that correspond to
		/// each other in a certain order), this will be the grouping.
        /// </summary>
        /// <value>The unit group.</value>
		public DisplayUnitGrouping? UnitGroup {get;set;}

        /// <summary>
        /// Sets the attributes on the DisplayUnit.
        /// </summary>
        /// <param name="attributes">Attributes.</param>
        abstract public void SetAttributes (Dictionary<string,string> attributes);


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

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>The date created.</value>
        public DateTime DateCreated {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the associated event's ID
		/// </summary>
		/// <value>The associated event.</value>
        public Guid AssociatedEvent {
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
            set {
                if (value < 0) {
                    throw new NegativePositionException ("Position of DisplayUnit must be positive.");
                }
                _positionInEvent = value;
            }
        }

		/// <summary>
		/// This creates a duplicate of this display unit with a brand new Id.
		/// </summary>
        public virtual DisplayUnit Clone(){
            var ctor = this.Plugin.DisplayUnitType.GetConstructor (new Type[]{ typeof(Dictionary<string,string>)});
            DisplayUnit unit = ctor.Invoke (new object[]{ this.GetAttributes () }) as DisplayUnit;
            unit.Name = this.Name;
            unit.Description = this.Description;
            unit.DateCreated = this.DateCreated;
            unit.AssociatedEvent = this.AssociatedEvent;
            unit.PositionInEvent = this.PositionInEvent + 1;
            unit.UnitGroup = this.UnitGroup;
            return unit;
        }

    }
}
