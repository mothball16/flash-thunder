using FlashThunder.GameLogic.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlashThunder.Managers;

public enum MouseButtonType
{
    Left,
    Middle,
    Right
}

/// <summary>
/// A generic MonoGame input manager that provides basic utilities for reading and
/// retrieving actions mapped to keys.
/// </summary>
/// <typeparam name="TActionEnum">The action enum to map keys to.</typeparam>
internal class InputManager<TActionEnum> : IInputState<TActionEnum> where TActionEnum : Enum
{
    private KeyboardState _kb, _prevKb;
    private MouseState _m, _prevM;

    private readonly Dictionary<Keys, TActionEnum> _actions;
    private readonly Dictionary<MouseButtonType, TActionEnum> _mActions;
    private readonly HashSet<TActionEnum> _active, _released, _activated;

    // communication
    public event Action<TActionEnum> OnActivated, OnReleased;

    public event Action<Point> OnMouseMoved;

    public event Action<int> OnMouseScrolled;

    public InputManager()
    {
        _actions = [];
        _mActions = [];
        _active = [];
        _released = [];
        _activated = [];
    }

    #region - - - [ Binding Methods ] - - -

    /// <summary>
    /// Binds an action to a key, overwriting any bind that may have previously existed.
    /// </summary>
    /// <param name="key">The key to bind the action to.</param>
    /// <param name="action">The action enum to be binded.</param>
    /// <returns>A reference to itself</returns>
    public InputManager<TActionEnum> BindAction(Keys key, TActionEnum action)
    {
        _actions[key] = action;
        return this;
    }

    /// <summary>
    /// Binds an action to a mouse button type, overwriting any bind that may have previously
    /// existed.
    /// </summary>
    /// <param name="type">The type to bind the action to.</param>
    /// <param name="action">The action enum to be binded.</param>
    /// <returns>A reference to itself</returns>
    public InputManager<TActionEnum> BindAction(MouseButtonType type, TActionEnum action)
    {
        _mActions[type] = action;
        return this;
    }

    /// <summary>
    /// Attempts to unbind an action from a key.
    /// </summary>
    /// <param name="key">The key mapping to unbind the action from.</param>
    /// <returns>Whether it was a success (Whether the key was mapped to begin with)</returns>
    public bool TryUnbindAction(Keys key)
        => _actions.Remove(key);

    /// <summary>
    /// Attempts to unbind an action from a mouse input type.
    /// </summary>
    /// <param name="type">The key mapping to unbind the action from.</param>
    /// <returns>Whether it was a success (Whether the key was mapped to begin with)</returns>
    public bool TryUnbindAction(MouseButtonType type)
        => _mActions.Remove(type);

    /// <summary>
    /// Attempts to retrieve an action mapped to a key.
    /// </summary>
    /// <param name="key">The key that is to be checked.</param>
    /// <param name="action">The action, if it exists under that keybind.</param>
    /// <returns>Whether an action existed under that bind.</returns>
    public bool TryGetAction(Keys key, out TActionEnum action)
        => _actions.TryGetValue(key, out action);

    /// <summary>
    /// Attempts to retrieve an action mapped to a mouse input type.
    /// </summary>
    /// <param name="type">The key that is to be checked.</param>
    /// <param name="action">The action, if it exists under that keybind.</param>
    /// <returns>Whether an action existed under that bind.</returns>
    public bool TryGetAction(MouseButtonType type, out TActionEnum action)
        => _mActions.TryGetValue(type, out action);

    /// <summary>
    /// Gets all actions binded to the manager.
    /// </summary>
    /// <returns>A read-only copy of the private actions dictionary.</returns>
    public ReadOnlyDictionary<Keys, TActionEnum> GetActions()
        => new(_actions);

    /// <summary>
    /// Gets all mouse actions binded to the manager.
    /// </summary>
    /// <returns>A read-only copy of the private mouse actions dictionary.</returns>
    public ReadOnlyDictionary<MouseButtonType, TActionEnum> GetMActions()
        => new(_mActions);

    /// <summary>
    /// Clears all actions binded ot the manager.
    /// </summary>
    /// <returns>Itself.</returns>
    public InputManager<TActionEnum> UnbindAll()
    {
        _actions.Clear();
        _mActions.Clear();
        return this;
    }

    #endregion - - - [ Binding Methods ] - - -

    #region - - - [ Core Methods ] - - -

    /// <summary>
    /// Updates the input data in the manager.
    /// </summary>
    /// <returns>Itself.</returns>
    public InputManager<TActionEnum> Update()
    {
        _prevKb = _kb;
        _kb = Keyboard.GetState();

        _prevM = _m;
        _m = Mouse.GetState();

        HandleKeys();
        HandleMouse();

        return this;
    }

    #endregion - - - [ Core Methods ] - - -

    #region - - - [ Helpers ] - - -

    private void HandleKeys()
    {
        // clear prev. input data
        _active.Clear();
        _activated.Clear();
        _released.Clear();

        // re-populate the input data
        foreach (var data in _actions)
        {
            var isDown = _kb.IsKeyDown(data.Key);
            var wasDown = _prevKb.IsKeyDown(data.Key);

            if (isDown)
            {
                // is pressed on this frame, add to active
                _active.Add(data.Value);

                if (!wasDown)
                {
                    // was previously inactive, add to (just) activated
                    _activated.Add(data.Value);
                    OnActivated?.Invoke(data.Value);
                }
            }
            else
            {
                if (wasDown)
                {
                    // was previously active, add to (just) released
                    _released.Add(data.Value);
                    OnReleased?.Invoke(data.Value);
                }
            }
        }
    }

    private void HandleMouse()
    {
        foreach (var data in _mActions)
        {
            var pressed = false;
            var lastPressed = false;

            switch (data.Key)
            {
                case MouseButtonType.Left:
                    pressed = _m.LeftButton == ButtonState.Pressed;
                    lastPressed = _prevM.LeftButton == ButtonState.Pressed;

                    break;

                case MouseButtonType.Right:
                    pressed = _m.RightButton == ButtonState.Pressed;
                    lastPressed = _prevM.RightButton == ButtonState.Pressed;

                    break;

                case MouseButtonType.Middle:
                    pressed = _m.MiddleButton == ButtonState.Pressed;
                    lastPressed = _prevM.MiddleButton == ButtonState.Pressed;
                    break;

                default:
                    throw new KeyNotFoundException(
                        $"An invalid MouseButtonType was provided. How??");
            }

            if (pressed && !lastPressed)
            {
                _activated.Add(data.Value);
                OnActivated?.Invoke(data.Value);
            }
            else if (!pressed && lastPressed)
            {
                _released.Add(data.Value);
                OnReleased?.Invoke(data.Value);
            }
        }

        // check MouseMoved
        if (_m.Position != _prevM.Position)
            OnMouseMoved?.Invoke(_m.Position);

        // check MouseSrolled
        int scrollDiff = _m.ScrollWheelValue - _prevM.ScrollWheelValue;
        if (scrollDiff != 0)
            OnMouseScrolled?.Invoke(scrollDiff);
    }

    #endregion - - - [ Helpers ] - - -

    #region - - - [ Polling Methods ] - - - -

    public Point MousePosition
        => _m.Position;

    public MouseState MouseState
        => _m;

    public IReadOnlySet<TActionEnum> Active
        => _active;

    public IReadOnlySet<TActionEnum> JustActivated
        => _activated;

    public IReadOnlySet<TActionEnum> JustReleased
        => _released;


    #endregion - - - [ Polling Methods ] - - - -
}