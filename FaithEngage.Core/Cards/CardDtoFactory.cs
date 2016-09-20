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
	/// <summary>
	/// Obtains RenderableCardDTOs from varous sources.
	/// </summary>
	public class CardDtoFactory : ICardDTOFactory
    {
        private readonly ITemplatingService _tempService;
        private readonly IPluginFileManager _fileMgr;

        public CardDtoFactory (ITemplatingService tempService, IPluginFileManager fileMgr)
        {
            _tempService = tempService;
            _fileMgr = fileMgr;
        }

        /// <summary>
        /// Obtains an array of RenderableCrdDTOs from a dictionary of display
        /// units, ordered by the integer key used.
        /// </summary>
        /// <returns>The cards.</returns>
        /// <param name="units">A dictionary of DisplayUnits with integer keys indicating
        /// their positions in the event.</param>
		public RenderableCardDTO[] GetCards(Dictionary<int,DisplayUnit> units)
        {
            return getCards(units)
                .Select (v => convert(v))
                .ToArray();
        }
		/// <summary>
		/// Obtains a single card from a DisplayUnit.
		/// </summary>
		/// <returns>The card.</returns>
		/// <param name="unit">Unit.</param>
		public RenderableCardDTO GetCard(DisplayUnit unit)
        {
            IRenderableCard card;
			try {
                //Get the files for the display unit's plugin
				var files = _fileMgr.GetFilesForPlugin (unit.Plugin.PluginId.Value);
                //Get the card from the display unit
				card = unit.GetCard (_tempService, files);
            } catch(Exception ex){
				return null;
			}
			return convert (card);
        }

        private IEnumerable<IRenderableCard> getCards(Dictionary<int,DisplayUnit> dict)
        {
            //Order the display units by their integer key value
			dict.OrderBy (p => p.Key);
            //Loop through the display units
			foreach(var du in dict.Values)
            {
                IRenderableCard card;
                try {
                    //Get the files for the du
					var files = _fileMgr.GetFilesForPlugin (du.Plugin.PluginId.Value);
                    //Get the card from the du
					card = du.GetCard(_tempService, files);
                }catch{ //If an error is encountered, fail silently and move on to th next.
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

		/// <summary>
		/// Converts an IRenderableCard to a RenderableCardDTO.
		/// </summary>
		/// <returns>The card.</returns>
		/// <param name="card">Card.</param>
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

