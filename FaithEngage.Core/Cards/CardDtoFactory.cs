using System;
using System.Linq;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Cards.Interfaces;
using System.IO;
using System.CodeDom;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;

namespace FaithEngage.Core.Cards
{
	public class CardDtoFactory : ICardDTOFactory
    {
        private readonly ITemplatingService _tempService;
        private readonly IPluginFileManager _fileMgr;

        public CardDtoFactory (ITemplatingService tempService, IPluginFileManager fileMgr)
        {
            _tempService = tempService;
            _fileMgr = fileMgr;
        }

        public RenderableCardDTO[] GetCards(Dictionary<int,DisplayUnit> units)
        {
            return getCards(units)
                .Select (v => convert(v))
                .ToArray();
        }

        public RenderableCardDTO GetCard(DisplayUnit unit)
        {
            IRenderableCard card;
			try {
                var files = _fileMgr.GetFilesForPlugin (unit.Plugin.PluginId.Value);
                card = unit.GetCard (_tempService, files);
            } catch(Exception ex){
				return null;
			}
			return convert (card);
        }

        private IEnumerable<IRenderableCard> getCards(Dictionary<int,DisplayUnit> dict)
        {
            dict.OrderBy (p => p.Key);
            foreach(var du in dict.Values)
            {
                IRenderableCard card;
                try {
                    var files = _fileMgr.GetFilesForPlugin (du.Plugin.PluginId.Value);
                    card = du.GetCard(_tempService, files);
                }catch{
                    continue;
                }
                yield return card;  
            }
        }

		public RenderableCardDTO ConvertCard (IRenderableCard card)
		{
			try {
				return convert (card);
			} catch {
				return null;	
			}
		}

        private RenderableCardDTO convert(IRenderableCard card)
        {
            var dto = new RenderableCardDTO ();
            dto.Title = card.Title;
            dto.Description = card.Description;
            dto.OriginatingDisplayUnit = card.OriginatingDisplayUnit.Id;
            dto.PositionInEvent = card.OriginatingDisplayUnit.PositionInEvent;
            dto.AssociatedEvent = card.OriginatingDisplayUnit.AssociatedEvent;
            var sections = new List<RenderableCardSectionDTO> ();
            foreach(var sec in card.Sections)
            {
                var secDto = new RenderableCardSectionDTO ();
                secDto.HeadingText = sec.HeadingText;
                secDto.HtmlContents = sec.HtmlContents;
                sections.Add (secDto);
            }
            dto.Sections = sections.ToArray ();
            return dto;
        }
    }
}

