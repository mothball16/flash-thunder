using fennecs;
using FlashThunder.Events.GameEvents;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Events;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashThunder.GameLogic.Commands;

internal sealed class NextTurnHandler : IDisposable
{
    private readonly World _world;
    private readonly IEventPublisher _notifier;
    private readonly List<IDisposable> _subscriptions;
    private readonly Stream<SkillSet, TeamTag> _skillsToDisableOrRefresh;

    public NextTurnHandler(World world)
    {
        _world = world;
        _notifier = world.Get<IEventPublisher>();
        _subscriptions = [
            world.Subscribe<NextTurnRequest>(Execute)
        ];
        _skillsToDisableOrRefresh = world.Query<SkillSet, TeamTag>().Stream();
    }
    public void Execute(NextTurnRequest _)
    {
        ref var _turnOrder = ref _world.Get<TurnOrderResource>();
        var order = _turnOrder.Order;

        // if we don't even have enough to cycle, don't cycle lol
        if (order.Count == 0)
        {
            Logger.Error("Not enough teams in turn order.");
            return;
        } else if (order.Count < _turnOrder.CurrentTeamIndex)
        {
            Logger.Warn("Turn order index prematurely outside the turn order count.");
        }

        // assuming this is the end of a turn, retrieve the team to cycle back
        var oldTeam = _turnOrder.CurTeam;

        // if the frontmost team doesn't have the current turn upon cycle request, we need to set
        // the first team as current instead of cycling (this may be the first turn of the game)
        if (!oldTeam.Has<IsCurrentTurn>())
        {
            oldTeam.Set(new IsCurrentTurn());
            _notifier.Publish(new TurnOrderChangedEvent(oldTeam, oldTeam));
            return;
        }

        // old teams turn is OVER. begin to cycle
        Logger.Print($"{_turnOrder.CurrentTeamIndex}, {oldTeam.Ref<TeamTag>().Team}");
        oldTeam.Remove<IsCurrentTurn>();
        _turnOrder.CurrentTeamIndex++;
        if(_turnOrder.CurrentTeamIndex >= order.Count)
        {
            Logger.Print("Cycling team order.");
           _turnOrder.CurrentTeamIndex = 0;
        }

        var newTeam = _turnOrder.CurTeam;
        var newTeamName = newTeam.Ref<TeamTag>().Team;
        Logger.Print($"{_turnOrder.CurrentTeamIndex}, {newTeamName}");
        newTeam.Add<IsCurrentTurn>();


        // turn-specific action components should now be ticked
        _skillsToDisableOrRefresh.For((ref SkillSet skillSet, ref TeamTag teamTag) =>
        {

            if (teamTag.Team == newTeamName)
            {
                foreach (var skill in skillSet.Skills)
                {
                    skill.State.CanUse = true;
                    skill.State.UsesLeftThisTurn = skill.Data.UsesPerTurn;
                    skill.State.TurnsSinceLastUse++;
                }
            } else
            {
                foreach (var skill in skillSet.Skills.Select(s => s.State))
                {
                    skill.CanUse = false;
                    skill.UsesLeftThisTurn = 0;
                }
            }
        });

        // notify the UI about the change
        _notifier.Publish(new TurnOrderChangedEvent(oldTeam, newTeam));
    }

    public void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
    }
}