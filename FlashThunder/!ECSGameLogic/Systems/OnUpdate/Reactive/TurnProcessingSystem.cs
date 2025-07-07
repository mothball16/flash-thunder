namespace FlashThunder.ECSGameLogic.Systems.OnUpdate.Set.Reactive;
using System;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.Events;
using FlashThunder.Extensions;
using FlashThunder.Factories;
using FlashThunder.Interfaces;
using FlashThunder.Managers;

[With(typeof(NextTurnRequestComponent))]
internal sealed class TurnProcessingSystem : AEntitySetSystem<GameFrameSnapshot>
{
    private readonly TurnOrderResource _turnOrder;
    private readonly IEventPublisher _publisher;
    public TurnProcessingSystem(World world, IEventPublisher publisher) : base(world)
    {
        _turnOrder = world.Get<TurnOrderResource>();
        _publisher = publisher;
    }

    /// <summary>
    /// Processes the turn order by cycling to the next team.
    /// </summary>
    private void Cycle()
    {
        var queue = _turnOrder.Order;

        // if we don't even have enough to cycle, don't cycle lol
        if (queue.Count <= 1)
        {
            Console.WriteLine("Not enough teams in turn order.");
            return;
        }

        // if the frontmost team doesn't have the current turn upon cycle request, we need to set
        // the first team as current instead of cycling
        var curTeam = queue.Peek();
        if (!curTeam.Has<IsCurrentTurnComponent>())
        {

            curTeam.Set(new IsCurrentTurnComponent());
            _publisher.Publish(new TurnOrderChangedEvent(curTeam));
            return;
        }

        // cycle the team order (we already stored curteam, so we don't need to store the dequeue result)
        queue.Dequeue();
        var nextTeam = queue.Peek();
        curTeam.Remove<IsCurrentTurnComponent>();
        nextTeam.Set(new IsCurrentTurnComponent());
        _publisher.Publish(new TurnOrderChangedEvent(nextTeam));
        queue.Enqueue(curTeam);
    }

    protected override void Update(GameFrameSnapshot state, in Entity entity)
    {
        Cycle();
        World.AddDebris(entity);
    }
}