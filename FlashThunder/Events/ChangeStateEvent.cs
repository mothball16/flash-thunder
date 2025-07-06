using System;

namespace FlashThunder.Events;

/// <summary>
/// An event used to notify listeners that a state change is requested.
/// </summary>
internal struct ChangeStateEvent
{
    /// <summary>
    /// The type of the state to transition to. Use typeof().
    /// </summary>
    public Type To { get; set; }
    /// <summary>
    /// The type of the state to transition from. Use typeof().
    /// </summary>
    public Type From { get; set; }
}
