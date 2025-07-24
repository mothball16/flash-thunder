using fennecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Attacks.Components
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
