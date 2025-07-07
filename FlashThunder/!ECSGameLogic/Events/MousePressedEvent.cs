using FlashThunder.Enums;
using FlashThunder.Managers;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly struct MousePressedEvent
{
    public readonly MouseButtonType Action;

    public MousePressedEvent(MouseButtonType action)
        => Action = action;
}
