using System;
using System.Collections.Generic;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.Cards.DefaultImplementations;

namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
{
    public class TextUnit : DisplayUnit
    {
        public TextUnit (Dictionary<string, string> attributes) : base (attributes)
        {
            string text;
            attributes.TryGetValue ("Text", out text);
            Text = text;
        }

        public TextUnit (Guid id, Dictionary<string, string> attributes) : base (id, attributes)
        {
            string text;
            attributes.TryGetValue ("Text", out text);
            Text = text;
        }

        public string Text { get; set; }

        public override DisplayUnitPlugin Plugin {
            get {
                return new TextUnitPlugin ();
            }
        }

        public override Dictionary<string, string> GetAttributes ()
        {
            return new Dictionary<string, string> { { "Text", this.Text}};
        }

        public override IRenderableCard GetCard (ITemplatingService service, IDictionary<Guid, PluginFileInfo> files)
        {
            var contents = service.CompileHtmlFromTemplateKey ("TextUnitCard", this);
            var card = new RenderableCard () 
            { 
                Description = Description, 
                OriginatingDisplayUnit = this, 
                Title = this.Name
            };

            var section = new RenderableCardSection () {
                HeadingText = this.Name,
                HtmlContents = contents
            };
            card.Sections = new RenderableCardSection [] { section };
            return card;
        }

        public override void SetAttributes (Dictionary<string, string> attributes)
        {
            string text;
            attributes.TryGetValue ("Text", out text);
            if (!string.IsNullOrWhiteSpace (text)) Text = text;
        }
    }
}

