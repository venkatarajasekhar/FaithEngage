using System.Collections.Generic;

namespace FaithEngage.Core.Cards.Interfaces
{
    public interface ICardEditorAction
    {
        Dictionary<string,string> DoAction(Dictionary<string,object>Parameters);
    }
}

