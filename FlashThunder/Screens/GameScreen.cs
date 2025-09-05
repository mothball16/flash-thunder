using fennecs;
using FlashThunder.Components;
using FlashThunder.Events.GameEvents;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Events;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.GameLogic.Selection.Events;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using Gum.Mvvm;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
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
        // make tabs invisible so that i dont have to toggle visibility every time i finish
        // an edit in Gum
        UnitInformation.Visible = false;
        NextTurnButton.Push += (s,a) => Presenter.NextTurnRequest();
    }

    public void Update(GameTime gameTime)
    {
        Presenter.Update();
    }
}

internal sealed class GameScreenPresenter : IDisposable
{
    private readonly TextureManager _textureManager;
    private readonly World _model;
    private readonly GameScreen _view;
    private readonly List<IDisposable> _disposables;
    private readonly Query _selected;
    
    private bool _showingSelectedUnitScreen;

    public GameScreenPresenter(TextureManager textureManager, World model, GameScreen view, EventBus eventBus)
    {
        _textureManager = textureManager;
        _model = model;
        _view = view;
        _selected = model.Query<SelectedTag>().Compile();
        _disposables = [
            eventBus.Subscribe<EntityCountChangedEvent>(view.OnEntityCountChanged),
            eventBus.Subscribe<TurnOrderChangedEvent>(msg => OnTurnOrderChanged(eventBus, msg)),
            eventBus.Subscribe<SelectedUnitAbilityChangedEvent>(msg => UpdateUnitAbilities(msg.SkillSet,msg.AbilityIndex))
        ];
    }

    #region - - - [ Unit Information ] - - -
    private void CheckUnitSelectionState()
    {
        // check here: do we need to display/undisplay the unit info?
        var somethingIsSelected = _selected.Count > 0;
        if (somethingIsSelected)
        {
            if (!_showingSelectedUnitScreen)
            {
                _showingSelectedUnitScreen = true;
                ReloadSelectedUnitInformation(_selected[0]);
            }
        }
        else
        {
            if (_showingSelectedUnitScreen)
            {
                _showingSelectedUnitScreen = false;
                HideSelectedUnitInformation();
            }
        }
    }

    private void ReloadSelectedUnitInformation(Entity e)
    {
        var whatsSelected = e.Has<AbilitySelected>()
            ? e.Ref<AbilitySelected>().AbilityIndex
            : -1;
        _view.UnitInformation.Visible = true;
        UpdateUnitHealthBar(e.Ref<Health>());
        UpdateUnitAbilities(e.Ref<SkillSet>(), whatsSelected);
    }

    private void UpdateUnitHealthBar(Health health)
    {
        var hpPercent = (float) health.CurHealth / health.MaxHealth;
        _view.HealthText.Text = $"HP: {health.CurHealth} / {health.MaxHealth}";
        _view.HealthBar.Width = Math.Clamp(hpPercent * 100,0,100);
    }

    private void UpdateUnitAbilities(SkillSet skillSet, int selectedAbility)
    {
        // clear previously displayed abilities
        for (int i = _view.AbilitiesContainer.Children.Count - 1; i >= 0; i--)
            (_view.AbilitiesContainer.Children[i] as InteractiveGue).RemoveFromRoot();

        for (int i = 0; i < skillSet.Skills.Count; i++)
        {
            var skill = skillSet.Skills[i];
            var abilityInstance = new AbilityLabelComponent();
            abilityInstance.Core.Y = skill.IsValid ? 0 : 20;
            abilityInstance.Icon.Texture = _textureManager.Get(skill.Data.Icon);
            abilityInstance.HotkeyText.Text = $"{i + 1}";
            abilityInstance.Selected.Visible = (selectedAbility == i);
            abilityInstance.ProgBar.Height = skill.IsValid
                ? 0
                : 100 - Math.Clamp((float) skill.State.TurnsSinceLastUse / Math.Max(1,skill.Data.CooldownBetweenTurns), 0, 100);
            _view.AbilitiesContainer.AddChild(abilityInstance.Visual);
        }
    }


    private void HideSelectedUnitInformation()
    {
        _view.UnitInformation.Visible = false;
    }

    #endregion

    #region - - - [ Turn Requests ] - - -
    public static void OnTurnOrderChanged(IEventPublisher publisher, TurnOrderChangedEvent msg)
    {
        if(msg.To == msg.From)
        {
            publisher.Publish(new MakePopupEvent($"{msg.To.Ref<TeamTag>().Team} begins turn."));
        }
        publisher.Publish(new MakePopupEvent($"{msg.From.Ref<TeamTag>().Team} ends turn. {msg.To.Ref<TeamTag>().Team} begins turn."));
    }

    public void NextTurnRequest()
    {
        Logger.Print("Reached nextturnrequest");
        _model.Get<InputResource>().DebounceActions();
        _model.Publish(new NextTurnRequest());
    }

    #endregion

    public void Update()
    {
        CheckUnitSelectionState();
    }

    public void Dispose()
    {
        _disposables.ForEach(d => d.Dispose());
    }
}