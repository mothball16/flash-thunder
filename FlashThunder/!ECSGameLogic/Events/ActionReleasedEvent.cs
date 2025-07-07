using FlashThunder.Enums;
using FlashThunder.Snapshots;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly record struct ActionReleasedEvent(GameAction Action, MouseSnapshot Mouse);
