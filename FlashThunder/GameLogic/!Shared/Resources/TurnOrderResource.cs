using fennecs;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Components
{
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
