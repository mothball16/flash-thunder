namespace FlashThunder.Events.GameEvents;

/// <summary>
/// An event used to notify listeners that a state change is requested.
/// </summary>
internal struct EntityCountChangedEvent
{
    public int Count { get; set; }
}
