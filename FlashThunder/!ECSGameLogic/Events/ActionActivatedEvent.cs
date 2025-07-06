using FlashThunder.Enums;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly struct ActionActivatedEvent
{
    public readonly GameAction Action;
    public ActionActivatedEvent(GameAction action) => Action = action;
}
