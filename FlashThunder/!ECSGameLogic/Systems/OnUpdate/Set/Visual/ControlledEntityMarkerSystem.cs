using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Components;
using MonoGame.Shapes;
using System;
using FlashThunder.ECSGameLogic.Components.UnitStats;
using FlashThunder.Extensions;
using FlashThunder.Defs;
using FlashThunder._ECSGameLogic;
using FlashThunder._ECSGameLogic.Components;

namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Units;

/// <summary>
/// Creates a marker underneath all controlled entities.
/// </summary>
[With(typeof(GridPosComponent), typeof(IdentifierComponent))]
internal sealed class ControlledEntityMarkerSystem : AEntitySetSystem<GameFrameSnapshot>
{
    private readonly TurnOrderResource _turnOrder;
    public ControlledEntityMarkerSystem(World world) : base(world)
    {
        _turnOrder = world.Get<TurnOrderResource>();
    }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        //guard clause -- if the entity is controlled (but not by us), skip it
        if (_turnOrder.Current != entity.Get<OwnableComponent>().Owner
            || entity.Get<IdentifierComponent>().ID == EntityID.ControlMarker)
        {
            return;
        }

        var pos = entity.Get<GridPosComponent>();
        World.RequestSpawn(EntityID.ControlMarker, pos.X, pos.Y);
    }
}