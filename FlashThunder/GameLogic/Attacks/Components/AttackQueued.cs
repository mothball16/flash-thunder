using FlashThunder.GameLogic.Attacks.Data;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Attacks.Components
{
    internal struct AttackQueued
    {
        public Queue<AttackData> Queue { get; set; }
    }
}
