using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Components;
using System;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

/// <summary>
/// Modifies the intents of all controlled units.
/// </summary>
[With(typeof(ControlledComponent), typeof(GridPosComponent))]
internal sealed class PlayerCommandSystem : AEntitySetSystem<GameFrameSnapshot>
{
    private readonly World _world;

    public bool IsEnabled { get; set; }

    public PlayerCommandSystem(World world) : base(world)
    {
        _world = world;
    }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        throw new NotSupportedException("PlayerCommandSystem is not implemented yet. " +
"This system is intended to modify the intents of all controlled units based on player input.");
    }
}