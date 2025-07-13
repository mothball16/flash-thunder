using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.Events;
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
    public GameScreenPresenter Presenter { get; set; }

    public void OnEntityCountChanged(EntityCountChangedEvent msg)
    {
        UnitCount.Text = $"Active units: {msg.Count}";
    }

    public void OnTurnOrderChanged(TurnOrderChangedEvent msg)
    {
        TurnOrder.Text = $"Current turn: {msg.To.Ref<TeamTag>().Team}";
    }
    partial void CustomInitialize()
    {
        //nothing yet..
    }
}

internal sealed class GameScreenPresenter
{
    private readonly GameScreen _view;
    private readonly IEventSubscriber _subscriber;

    public GameScreenPresenter(GameScreen view, IEventSubscriber subscriber)
    {
        _view = view;
        _subscriber = subscriber;

        subscriber.Subscribe<EntityCountChangedEvent>(view.OnEntityCountChanged);
        subscriber.Subscribe<TurnOrderChangedEvent>(view.OnTurnOrderChanged);
    }
}