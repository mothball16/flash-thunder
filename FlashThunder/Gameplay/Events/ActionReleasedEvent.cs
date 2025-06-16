using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Events
{
    public struct ActionReleasedEvent
    {
        public GameAction action;

        public ActionReleasedEvent(GameAction action) { this.action = action; }
    }
}
