using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Events
{
    public readonly struct ActionActivatedEvent
    {
        public readonly GameAction Action;
        public ActionActivatedEvent(GameAction action) => Action = action;
    }
}
