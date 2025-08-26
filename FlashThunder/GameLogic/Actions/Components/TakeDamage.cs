using fennecs;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Actions.Components
{
    public record TakeDamageEntry(Entity From, int Amount);
    internal struct TakeDamage
    {
        public List<TakeDamageEntry> Inflicts { get; set; }
        public TakeDamage()
        {
            Inflicts = [];
        }
    }
}
