using fennecs;
using FlashThunder.Events.GameEvents;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlashThunder.Screens;

internal partial class GameScreen : IUpdateScreen
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

    public void Update(GameTime gameTime)
    {
        Presenter.Update();
    }
}

internal sealed class GameScreenPresenter : IDisposable
{
    private readonly GameScreen _view;
    private readonly List<IDisposable> _disposables;
    private readonly Query _selected;

    private bool _showingSelectedUnitScreen;

    public GameScreenPresenter(World model, GameScreen view, IEventSubscriber subscriber)
    {
        _view = view;
        
        _selected = model.Query<SelectedTag>().Compile();
        _disposables = [
            subscriber.Subscribe<EntityCountChangedEvent>(view.OnEntityCountChanged),
            subscriber.Subscribe<TurnOrderChangedEvent>(view.OnTurnOrderChanged)
        ];
    }

    private void DisplaySelectedUnitInformation(Entity e)
    {
        _view.UnitInformation.Visible = true;
        UpdateHealthBar(e.Ref<Health>());
    }

    private void UpdateHealthBar(Health health)
    {
        var hpPercent = (float) health.CurHealth / health.MaxHealth;
        _view.HealthText.Text = $"HP: {health.CurHealth} / {health.MaxHealth}";
        _view.HealthBar.Width = Math.Clamp(hpPercent * 100,0,100);
    }

    private void HideSelectedUnitInformation()
    {
        _view.UnitInformation.Visible = false;
    }

    public void Update()
    {
        var somethingIsSelected = _selected.Count > 0;
        if (somethingIsSelected)
        {
            if (!_showingSelectedUnitScreen)
            {
                _showingSelectedUnitScreen = true;
                DisplaySelectedUnitInformation(_selected[0]);
            }
        } else
        {
            if (_showingSelectedUnitScreen)
            {
                _showingSelectedUnitScreen = false;
                HideSelectedUnitInformation();
            }

        }
    }

    public void Dispose()
    {
        _disposables.ForEach(d => d.Dispose());
    }
}