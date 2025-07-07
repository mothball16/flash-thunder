using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.ECSGameLogic.Resources;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Core;

/// <summary>
/// Manages and disposes of entities marked for deletion.
/// </summary>
[With(typeof(DestroyRequestComponent))]
internal sealed class EntityFinalDisposalSystem : AEntitySetSystem<GameFrameSnapshot>
{
    public EntityFinalDisposalSystem(World world) : base(world) { }
    protected override void Update(GameFrameSnapshot _, in Entity entity)
    {
        entity.Dispose();
    }
}