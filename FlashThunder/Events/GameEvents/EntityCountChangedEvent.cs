namespace FlashThunder.Events.GameEvents;

/// <summary>
/// An event used to notify listeners that a state change is requested.
/// </summary>
internal readonly record struct EntityCountChangedEvent(int Count);