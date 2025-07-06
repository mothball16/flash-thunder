using System;
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
[With(typeof(ToDestroyInFramesComponent))]
internal sealed class EntityDisposalInFramesTickerSystem : AEntitySetSystem<GameFrameSnapshot>
{

    public EntityDisposalInFramesTickerSystem(World world) : base(world) { }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        ref var destroyComp = ref entity.Get<ToDestroyInFramesComponent>();
        if (destroyComp.Lifetime <= 0)
            entity.Set(new ToDestroyComponent());
        destroyComp.Lifetime--;
    }
}

/*
foreach (var e in _entitySetSeconds.GetEntities())
{
ref var destroyComp = ref e.Get<ToDestroyInSecondsComponent>();
if (destroyComp.Lifetime <= 0)
    e.Set(new ToDestroyComponent());

destroyComp.Lifetime -= dt;
}
*/