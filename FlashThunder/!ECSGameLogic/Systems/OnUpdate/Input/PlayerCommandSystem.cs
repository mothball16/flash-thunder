using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Components;
using System;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

/// <summary>
/// Modifies the intents of all controlled units.
/// </summary>
[With(typeof(OwnableComponent), typeof(GridPosComponent))]
internal sealed class PlayerCommandSystem : AEntitySetSystem<GameFrameSnapshot>
{

    public PlayerCommandSystem(World world) : base(world) { }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        // This system isn't implemented yet.
    }
}