using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using FlashThunder.Managers;
using FlashThunder.Factories;
using FlashThunder._ECSGameLogic;
using FlashThunder.Enums;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;
using FlashThunder.Snapshots;
using Microsoft.Xna.Framework.Input;
using Dcrew.MonoGame._2D_Camera;
using Microsoft.Xna.Framework;

namespace FlashThunder.Core;

internal sealed class GameContext : IDisposable
{
    // outside context runtime
    public TexManager AssetManager { get; init; }
    public InputManager<GameAction> InputManager { get; init; }

    // inside context runtime
    public InputMediator InputBridge { get; init; }
    public World World { get; init; }
    public EntityFactory Factory { get; init; }
    public SequentialSystem<GameFrameSnapshot> UpdateSystems { get; init; }
    public SequentialSystem<DrawFrameSnapshot> DrawSystems { get; init; }
    public ActionSnapshotCreator ActionSnapshotCreator { get; init; }
    public Camera Camera { get; init; }

    public ActionSnapshot CreateActionSnapshot()
    {
        return ActionSnapshotCreator.GetSnapshot();
    }

    public MouseSnapshot CreateMouseSnapshot(MouseSnapshot last)
    {
        // pull out a bunch of mouse shit

        var state = InputManager.GetMouseState();
        var pos = InputManager.GetMousePosition();
        var diff = pos - last.Position;
        var delta = (float) Math.Sqrt((diff.X * diff.X) + (diff.Y * diff.Y));

        // poll the mouse state for button presses
        var lPressed = state.LeftButton == ButtonState.Pressed;
        var mPressed = state.MiddleButton == ButtonState.Pressed;
        var rPressed = state.RightButton == ButtonState.Pressed;

        // update mouse positions
        // (screen: mouse pixel on screen)
        // (world: mouse position from the world origin)
        // (tile: mouse position in world tile coordinates)
        var worldPos = Camera.ScreenToWorld(pos);
        var tilePos = new Point(
            worldPos.X / GameConstants.TileSize,
            worldPos.Y / GameConstants.TileSize);

        return new MouseSnapshot(
            diff, delta, 0f, pos, worldPos, tilePos, lPressed, mPressed, rPressed);
    }


    /// <summary>
    /// Nukes the GameContext. Should properly cleanup any game-runtime specific systems/objects.
    /// </summary>
    public void Dispose()
    {
        World.Dispose();
        InputBridge?.Dispose();
        UpdateSystems?.Dispose();
        DrawSystems?.Dispose();
        Camera?.Dispose();
        ActionSnapshotCreator?.Dispose();
        GC.SuppressFinalize(this);
    }
}