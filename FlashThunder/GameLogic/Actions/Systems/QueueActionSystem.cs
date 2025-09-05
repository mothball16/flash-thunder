using fennecs;
using Microsoft.Xna.Framework;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Selection.Components;
using System;
using FlashThunder.Utilities;
using FlashThunder.Core;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Actions.Data;

namespace FlashThunder.GameLogic.Actions.Systems;

internal sealed class QueueActionSystem : AUpdateSystem<float>
{
    private readonly Stream<SkillSet, AbilitySelected, ActionTiles> _selectedActionReadyEntities;
    private readonly World _world;
    public QueueActionSystem(World world)
    {
        _selectedActionReadyEntities = world.Query<SkillSet, AbilitySelected, ActionTiles>()
            .Has<SelectedTag>()
            .Not<ExecutingActionTag>()
            .Stream();

        _world = world;
    }

    public override void Update(float upd)
    {
        var input = _world.Get<InputResource>();

        // if select action didn't happen, don't do anything
        if (!input.WasJustActivated(GameAction.Action))
            return;

        var mouse = _world.Get<MouseResource>();
        var mousePos = new Point(mouse.TileX, mouse.TileY);

        // - - - [ figure out whether the tile is valid ] - - -
        _selectedActionReadyEntities.For(
            (in Entity e, ref SkillSet skillSet, ref AbilitySelected abilitySelected, ref ActionTiles tiles) =>
            {
                var skill = skillSet[abilitySelected.AbilityIndex];
                if (tiles.Tiles.TryGetValue(mousePos, out var waypoints))
                {
                    AttackQueued queuedActions;
                    if (!e.Has<AttackQueued>())
                    {
                        queuedActions = new AttackQueued();
                        e.Add(queuedActions);
                    }
                    else
                    {
                        queuedActions = e.Ref<AttackQueued>();
                    }
                    skill.State.UsesLeftThisTurn--;
                    queuedActions.Queue.Enqueue(new ActionData(
                        e,
                        skill.Data.AttackBehavior,
                        skill.Data.AttackParams,
                        waypoints));
                }
            });
    }
}