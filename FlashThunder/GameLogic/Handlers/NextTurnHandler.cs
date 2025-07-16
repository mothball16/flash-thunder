using fennecs;
using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Events;
using FlashThunder.Extensions;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Events;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace FlashThunder.GameLogic.Commands;

internal sealed class NextTurnHandler : IDisposable
{
    private readonly World _world;
    private readonly IEventPublisher _uiNotifier;
    private readonly List<IDisposable> _subscriptions;
    public NextTurnHandler(World world, IEventPublisher uiNotifier)
    {
        _world = world;
        _uiNotifier = uiNotifier;
        _subscriptions = [
            world.Subscribe<NextTurnRequestEvent>(Execute)
        ];
    }
    public void Execute(NextTurnRequestEvent _)
    {
        ref var _turnOrder = ref _world.GetResource<TurnOrderResource>();
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
            _uiNotifier.Publish(new TurnOrderChangedEvent(oldTeam));
            return;
        }

        Logger.Print($"{_turnOrder.CurrentTeamIndex}, {oldTeam.Ref<TeamTag>().Team}");
        oldTeam.Remove<IsCurrentTurn>();
        _turnOrder.CurrentTeamIndex++;
        if(_turnOrder.CurrentTeamIndex >= order.Count)
        {
            Logger.Print("Cycling team order.");
           _turnOrder.CurrentTeamIndex = 0;
        }

        var newTeam = _turnOrder.CurTeam;

        Logger.Print($"{_turnOrder.CurrentTeamIndex}, {newTeam.Ref<TeamTag>().Team}");
        newTeam.Add<IsCurrentTurn>();

        // notify the UI about the change
        _uiNotifier.Publish(new TurnOrderChangedEvent(newTeam));
    }

    public void Dispose()
    {
        _subscriptions.ForEach(s => s.Dispose());
    }
}