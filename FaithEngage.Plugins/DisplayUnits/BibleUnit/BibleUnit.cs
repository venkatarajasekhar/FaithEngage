using System;
using System.Collections.Generic;
using FaithEngage.Core.Cards;
using FaithEngage.Core.Interfaces;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Plugins.DisplayUnits.BibleUnitPlugin
{
    public class BibleUnit : DisplayUnit
    {
        private readonly IReferenceProvider _refProvider;

        private string _rawReference;
        private IReference _reference;

        public BibleUnit (Dictionary<string, string> attributes) : base (attributes)
        {
        }
        

        public BibleUnit (Guid id, Dictionary<string, string> attributes) : base (id, attributes)
        {
        }

        #region implemented abstract members of DisplayUnit
        public override Dictionary<string, string> GetAttributes ()
        {
            throw new NotImplementedException ();
        }
        public override FaithEngage.Core.Cards.Interfaces.IRenderableCard GetCard ()
        {
            throw new NotImplementedException ();
        }
        public override CardActionResult ExecuteCardAction (CardAction Action)
        {
            throw new NotImplementedException ();
        }
        public override void SetAttributes (Dictionary<string, string> attributes)
        {
            throw new NotImplementedException ();
        }
        public override DisplayUnitPlugin Plugin {
            get {
                throw new NotImplementedException ();
            }
        }
        #endregion        
              

        private void tryParseScripture(string reference)
        {
            _reference = _refProvider.Parse (reference);
        }

        public string GetReference(){
            return _refProvider.GetReference (_reference);
        }


    }
}

