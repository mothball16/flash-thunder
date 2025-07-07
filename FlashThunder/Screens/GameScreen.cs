using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace FlashThunder.Screens;

internal partial class GameScreen
{
    public GameScreen(IEventSubscriber subscriber)
    {
        subscriber.Subscribe<EntityCountChangedEvent>(OnEntityCountChanged);
        subscriber.Subscribe<TurnOrderChangedEvent>(OnTurnOrderChanged);
    }

    private void OnEntityCountChanged(EntityCountChangedEvent msg)
    {
        UnitCount.Text = $"Active units: {msg.Count}";
    }

    private void OnTurnOrderChanged(TurnOrderChangedEvent msg)
    {
        TurnOrder.Text = $"Current turn: {msg.To.Get<TeamTagComponent>().Team}";
    }
    partial void CustomInitialize()
    {
        //nothing yet..
    }
}
