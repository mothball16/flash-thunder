// this isn't actually a resource, but just a read-only interface for the inputmanager

using FlashThunder.Enums;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace FlashThunder.GameLogic.Input.Resources;

internal struct InputResource
{
    public IInputState<GameAction> Input { get; init; }
    public HashSet<GameAction> ConsumedInputs { get; set; }
    public bool Debounced { get; set; }

    public readonly bool IsValidAction(GameAction action)
        => !Debounced && !ConsumedInputs.Contains(action);

    public readonly bool WasJustActivated(GameAction action)
        => IsValidAction(action) && Input.JustActivated.Contains(action);

    public readonly bool IsActivated(GameAction action)
        => IsValidAction(action) && Input.Active.Contains(action);

    public readonly bool WasJustReleased(GameAction action)
        => IsValidAction(action) && Input.JustReleased.Contains(action);

    public readonly bool UseAction(GameAction action)
        => ConsumedInputs.Add(action);

    // used if we are doing something like a button click where it's not necessarily certain that
    // anything is binded to LMB, but we still know that we shouldn't let actions simultaneously
    // occur with UI interaction.
    // might want to look into how this is usually handled though because this seems pretty hacky
    public void DebounceActions()
    {
        Debounced = true;
    }

    public void ResetActions()
    {
        Debounced = false;
        ConsumedInputs.Clear();
    }
}