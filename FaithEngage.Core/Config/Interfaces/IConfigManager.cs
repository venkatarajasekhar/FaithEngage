using System;
namespace FaithEngage.Core.Config
{
    public interface IConfigManager
    {
        string TempFolderPath { get; set;}
        string PluginsFolderPath { get; set;}



        string GetValue (string key);
        string SetValue (string Key, string value);


    }
}

