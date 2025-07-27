using Microsoft.Xna.Framework;
using System;
using System.Collections.ObjectModel;

namespace FlashThunder.GameLogic.Input.Resources;

public interface IInputState<TActionEnum> where TActionEnum : Enum
{
    bool IsActive(TActionEnum action);
    bool IsActivated(TActionEnum action);
    bool IsReleased(TActionEnum action);
    ReadOnlyCollection<TActionEnum> Active { get; }
    ReadOnlyCollection<TActionEnum> Activated { get; }
    ReadOnlyCollection<TActionEnum> Released { get; }
    Vector2 MousePosition { get; }
    Vector2 MouseDelta { get; }
}