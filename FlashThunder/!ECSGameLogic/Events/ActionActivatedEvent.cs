using FlashThunder.Enums;
using FlashThunder.Snapshots;

namespace FlashThunder.ECSGameLogic.Events;

internal readonly record struct ActionActivatedEvent(GameAction Action, MouseSnapshot Mouse);
