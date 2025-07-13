using fennecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Components
{
    [ComponentName("turnOrder")]
    internal struct TurnOrderResource
    {
        public List<Entity> Order { get; set; }
        public int CurrentTeamIndex { get; set; }
        public int RoundNumber { get; set; }

        public readonly Entity CurTeam
            => Order[CurrentTeamIndex];

        public TurnOrderResource()
        {
            Order = [];
        }
    }
}
