using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Events
{
    public readonly struct ActionReleasedEvent
    {
        public readonly GameAction Action;

        public ActionReleasedEvent(GameAction action) => Action = action;
    }
}
