using FlashThunder.Components;
using FlashThunder.Events.GameEvents;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashThunder.Screens;

public partial class PopupScreen : IUpdateScreen
{
    public PopupScreenPresenter Presenter { get; set; }
    partial void CustomInitialize()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        Presenter.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
    }


}

public sealed class PopupScreenPresenter : IDisposable
{
    private readonly PopupScreen _view;
    private readonly List<IDisposable> _disposables;
    private readonly List<PopupComponent> _popups;
    public PopupScreenPresenter(PopupScreen view, IEventSubscriber subscriber)
    {
        _view = view;
        _disposables = [
            subscriber.Subscribe<MakePopupEvent>(msg => OnPopupRequest(msg.Text, 3))
            ];
        _popups = [];
    }

    private void OnPopupRequest(string msg, float time)
    {
        var popup = new PopupComponent();
        popup.Message.Text = msg;
        popup.Lifetime = time;
        popup.Initialize();
        _view.MessageContainer.AddChild(popup.Visual);
        _popups.Add(popup);
    }
    public void Update(float dt)
    {
        
        for(int i = _popups.Count - 1; i >= 0; i--)
        {
            var popup = _popups[i];
            popup.Update(dt);
            if(popup.Lifetime <= 0)
            {
                _popups.RemoveAt(i);
                popup.RemoveFromRoot();
            }
        }
    }

    public void Dispose()
        => _disposables.ForEach(x => x.Dispose());
}


