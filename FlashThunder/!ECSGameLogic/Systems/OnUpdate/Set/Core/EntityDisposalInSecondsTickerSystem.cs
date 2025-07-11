﻿using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Events;
using FlashThunder.ECSGameLogic.Resources;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;

/// <summary>
/// Manages and disposes of entities marked for deletion.
/// </summary>
[With(typeof(DestroyInSecondsRequestComponent))]
internal sealed class EntityDisposalInSecondsTickerSystem : AEntitySetSystem<GameFrameSnapshot>
{
    public EntityDisposalInSecondsTickerSystem(World world) : base(world)
    {
    }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        ref var destroyComp = ref entity.Get<DestroyInSecondsRequestComponent>();
        if (destroyComp.Lifetime <= 0)
            entity.Set(new DestroyRequestComponent());
        destroyComp.Lifetime -= state.DeltaTime;
    }
}