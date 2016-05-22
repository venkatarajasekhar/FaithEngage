using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.DisplayUnits
{
    public class DisplayUnitDTO
    {
        public DisplayUnitDTO (Guid displayUnitId, Guid eventId)
        {
            Id = displayUnitId;
            AssociatedEvent = eventId;
            Attributes = new Dictionary<string,string> ();
        }

        public Guid Id {
            get;
            set;
        }

        public DateTime DateCreated {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public string PluginId {
            get;
            set;
        }


        public Dictionary<string,string> Attributes {
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
            set{
                if(value < 0){
                    throw new NegativePositionException("Position of DisplayUnit must be positive.");
                }
                _positionInEvent = value;
            }
        }

        public int PositionInGroup{get;set;}
        public Guid GroupId{get;set;}
    }
}

