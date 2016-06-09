using System;
using System.Collections.Generic;
using FaithEngage.Core.Cards;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;


namespace FaithEngage.Core.DisplayUnits
{
    /// <summary>
    /// This is the base display unit from which all other units are derived.
    /// </summary>
    abstract public class DisplayUnit
    {
            
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractClasses.DisplayUnit"/> class.
        /// </summary>
        protected DisplayUnit (Dictionary<string,string> attributes)
        {
            this.Id = Guid.NewGuid ();
            tryApplyAttributes (attributes);
        }

        protected DisplayUnit(Guid id, Dictionary<string,string> attributes)
        {
            if (id == Guid.Empty)
                throw new EmptyGuidException ();
            this.Id = id;
            tryApplyAttributes (attributes);
        }

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
        /// Gets an ordered array of IRenderableCards that will be rendered.
        /// </summary>
        /// <returns>The cards.</returns>
        abstract public IRenderableCard GetCard ();

        /// <summary>
        /// If a display unit type has actions that should be performed as response to
        /// interaction with a renderable card (i.e. external service calls, etc...),
        /// it will be sent to this method. These calls will be made via a respons API.
        /// </summary>
        /// <param name="Action">Action.</param>
        public virtual void ExecuteCardAction (CardAction Action)
        {
        }

        public virtual event CardActionResultEventHandler OnCardActionResult;

        public DisplayUnitGrouping? UnitGroup {get;set;}

        /// <summary>
        /// Each display unit type must know how to receive a dictionary of attributes
        /// and apply its contents.
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

        public DateTime DateCreated {
            get;
            set;
        }


        public Guid AssociatedEvent {
            get;
            set;
        }

        private int _positionInEvent;
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
