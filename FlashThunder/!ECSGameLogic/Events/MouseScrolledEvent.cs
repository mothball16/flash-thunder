using FlashThunder.Enums;
using FlashThunder.Managers;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly struct MouseScrolledEvent
{
    public readonly int Change;

    public MouseScrolledEvent(int change)
        => Change = change;
}
