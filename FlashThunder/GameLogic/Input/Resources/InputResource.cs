// this isn't actually a resource, but just a read-only interface for the inputmanager

using FlashThunder.Enums;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Input.Resources;

internal struct InputResource
{
    public IInputState<GameAction> Input { get; init; }
    public HashSet<GameAction> ConsumedInputs { get; set; }

    public readonly bool WasJustActivated(GameAction action)
        => !ConsumedInputs.Contains(action) && Input.JustActivated.Contains(action);

    public readonly bool IsActivated(GameAction action)
        => !ConsumedInputs.Contains(action) && Input.Active.Contains(action);

    public readonly bool WasJustReleased(GameAction action)
        => !ConsumedInputs.Contains(action) && Input.JustReleased.Contains(action);

    public readonly bool UseAction(GameAction action)
        => ConsumedInputs.Add(action);
}