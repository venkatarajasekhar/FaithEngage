using System;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins
{
    public class DisplayUnitPluginDTO
    {
        public Guid? Id {
            get;
            set;
        }

        public string AssemblyLocation {
            get;
            set;
        }

        public string FullName {
            get;
            set;
        }

        public string PluginName{get;set;}
        public int[] PluginVersion{ get; set;}

    }
}

