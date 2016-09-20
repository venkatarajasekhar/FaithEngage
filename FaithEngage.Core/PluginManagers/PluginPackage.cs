using System;
namespace FaithEngage.Core.PluginManagers
{
    public class PluginPackage
    {

        public class pluginInfo
        {
            public string PluginTypeName { get; set; }
            public PluginTypeEnum PluginTypeEnum { get; set; }
            public string DllName { get; set;}
            public string [] Files { get; set;}
        }

        public pluginInfo[] Plugins {
            get;
            set;
        }



    }
}

