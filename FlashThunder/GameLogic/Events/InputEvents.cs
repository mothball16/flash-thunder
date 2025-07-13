using FlashThunder.Enums;
using FlashThunder.GameLogic.Resources;
using FlashThunder.Managers;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly record struct ActionActivatedEvent(GameAction Action, MouseResource Mouse);
internal readonly record struct ActionReleasedEvent(GameAction Action, MouseResource Mouse);
internal readonly record struct MousePressedEvent(MouseButtonType Action, MouseResource Mouse);
internal readonly record struct MouseReleasedEvent(MouseButtonType Action, MouseResource Mouse);
internal readonly record struct MouseScrolledEvent(int Change);