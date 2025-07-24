using FlashThunder.GameLogic.Attacks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Attacks.Components
{
    internal struct AttackQueued
    {
        public Queue<AttackData> Queue { get; set; }
    }
}
