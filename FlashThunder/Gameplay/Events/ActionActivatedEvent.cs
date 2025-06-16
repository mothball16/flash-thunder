using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Events
{
    public struct ActionActivatedEvent
    {
        public GameAction action;
        public ActionActivatedEvent(GameAction action) { this.action = action; }
    }
}
