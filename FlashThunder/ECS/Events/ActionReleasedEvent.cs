using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.ECS.Events
{
    public struct ActionReleasedEvent
    {
        public PlayerAction action;

        public ActionReleasedEvent(PlayerAction action) { this.action = action; }
    }
}
