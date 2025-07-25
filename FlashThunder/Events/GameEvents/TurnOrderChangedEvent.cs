﻿using fennecs;

namespace FlashThunder.Events.GameEvents;

/// <summary>
/// An event used to notify listeners that the turn has changed.
/// </summary>
internal readonly record struct TurnOrderChangedEvent(Entity To);