using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.Enums;
using FlashThunder.Snapshots;
using Microsoft.Xna.Framework;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

internal sealed class PlayerCameraInputSystem : ISystem<GameFrameSnapshot>
{
    private readonly World _world;
    public bool IsEnabled { get; set; }
    public PlayerCameraInputSystem(World world)
        => _world = world;

    public void Update(GameFrameSnapshot state)
    {
        var actionsHeld = state.Actions.Active;
        ref var camResource = ref _world.Get<CameraResource>();

        var camSpeed = actionsHeld.Contains(GameAction.SpeedUpCamera)
            ? 16
            : 4;

        if (actionsHeld.Contains(GameAction.MoveLeft))
        {
            camResource.Target += new Vector2(-camSpeed, 0);
        }

        if (actionsHeld.Contains(GameAction.MoveRight))
        {
            camResource.Target += new Vector2(camSpeed, 0);
        }

        if (actionsHeld.Contains(GameAction.MoveUp))
        {
            camResource.Target += new Vector2(0, -camSpeed);
        }

        if (actionsHeld.Contains(GameAction.MoveDown))
        {
            camResource.Target += new Vector2(0, camSpeed);
        }

        // Panning movement
        if (state.Mouse.MPressed)
        {
            camResource.Target -= state.Mouse.MouseDiff.ToVector2();
        }
    }

    public void Dispose()
    {
    }
}