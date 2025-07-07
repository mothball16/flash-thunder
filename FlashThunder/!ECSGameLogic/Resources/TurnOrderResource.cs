using DefaultEcs;
using System.Collections.Generic;

namespace FlashThunder.ECSGameLogic.Resources;

/// <summary>
/// Global component for tracking the turn order of active teams.
/// </summary>
internal class TurnOrderResource
{
    public Queue<Entity> Order { get; }
    public TurnOrderResource()
    {
        Order = [];
    }

    public Entity Current => Order.Peek();
}
