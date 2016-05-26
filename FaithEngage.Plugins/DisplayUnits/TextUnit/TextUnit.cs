using System;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Plugins.DisplayUnits.TextUnitPlugin;
using FaithEngage.Core.Cards.DefaultImplementations;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Plugins.DisplayUnits.TextUnitPlugin
{
    public class TextUnit : DisplayUnit
    {
        public string Text {
            get;
            set;
        }
            
        public TextUnit (Dictionary<string, string> attributes) : base (attributes)
        {
            SetAttributes (attributes);
            Plugin = new TextUnitPlugin ();
        }
        

        public TextUnit (Guid id, Dictionary<string, string> attributes) : base (id, attributes)
        {
            SetAttributes (attributes);
            Plugin = new TextUnitPlugin ();
        }
        


//        public TextUnit (string text) : base()
//        {
//            this.Text = text;
//        }
//        public TextUnit (Guid guid, string text) : base(guid)
//        {
//            this.Text = text;
//        }

        #region implemented abstract members of DisplayUnit

        public override Dictionary<string, string> GetAttributes ()
        {
            return new Dictionary<string,string> (){ { "Text",Text } };
        }

        /// <summary>
        /// TextUnit produces a single card.
        /// </summary>
        /// <returns>The cards.</returns>
        public override IRenderableCard GetCard ()
        {
            var card = new TextCard ();
            card.Title = this.Name;
            card.Description = this.Description;
            card.OriginatingDisplayUnit = this;
            var section = new RenderableCardSection ();
            section.HeadingText = card.Title;
            section.HtmlContents = this.Text;
            card.Sections = new IRenderableCardSection[]{ section };
            return card;
        }

        public override CardActionResult ExecuteCardAction (CardAction Action)
        {
            return null;
        }

        public override void SetAttributes (Dictionary<string, string> attributes)
        {
            string text;
            attributes.TryGetValue ("Text", out text);
            Text = text;
        }
            

        public override DisplayUnitPlugin Plugin {
            get;
        }

        #endregion
    }
}

