using FlashThunder.Enums;
using FlashThunder.Managers;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly struct MouseReleasedEvent
{
    public readonly MouseButtonType Action;

    public MouseReleasedEvent(MouseButtonType action)
        => Action = action;
}
