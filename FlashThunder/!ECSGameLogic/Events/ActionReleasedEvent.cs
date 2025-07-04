﻿using FlashThunder.Enums;

namespace FlashThunder.ECSGameLogic.Events
{
    public readonly struct ActionReleasedEvent
    {
        public readonly GameAction Action;

        public ActionReleasedEvent(GameAction action) => Action = action;
    }
}
