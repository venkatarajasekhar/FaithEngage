using System;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core.DisplayUnitEditor
{
    public class DisplayUnitEditorContext
    {
        public DisplayUnit DisplayUnit {
            get;
            set;
        }

        public User CurrentUser{
            get;
            set;
        }

        public Organization CurrentOrg { get; set;}

    }
}

