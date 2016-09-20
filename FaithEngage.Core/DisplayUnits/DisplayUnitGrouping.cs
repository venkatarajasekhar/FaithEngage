using System;

namespace FaithEngage.Core.DisplayUnits
{
    public struct DisplayUnitGrouping
    {
        public DisplayUnitGrouping (int position, Guid id)
        {
            Position = position;
            Id = id;
        }

        public int Position;
        public Guid Id;
    }
}

