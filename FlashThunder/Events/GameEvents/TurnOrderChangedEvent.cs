using DefaultEcs;
using System;

namespace FlashThunder.Events;

/// <summary>
/// An event used to notify listeners that the turn has changed.
/// </summary>
internal readonly record struct TurnOrderChangedEvent(Entity To);