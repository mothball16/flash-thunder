using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Screens;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using System.Collections.Generic;

namespace FlashThunder.Managers;

internal sealed class UIManager : IDisposable
{
    private static Point OriginalUIDimensions = new(1920, 1080);
    
    private static GumService Gum => GumService.Default;
    private readonly Dictionary<ScreenLayer, GraphicalUiElement> _layers;
    private readonly EventBus _eventBus;
    public UIManager(EventBus eventBus)
    {
        _layers = [];
        _eventBus = eventBus;
        _eventBus.Subscribe<LoadScreenEvent>(OnLoadRequest);
    }


    public UIManager RescaleUIToResolution(GameWindow window)
    {
        window.AllowUserResizing = true;
        var zoom = window.ClientBounds.Height / (float)OriginalUIDimensions.Y;
        Gum.Renderer.Camera.Zoom = zoom;
        GraphicalUiElement.CanvasWidth = OriginalUIDimensions.X / zoom;
        GraphicalUiElement.CanvasHeight = OriginalUIDimensions.Y / zoom;
        return this;
    }


    public UIManager SetupListeners(GameWindow window)
    {
        window.ClientSizeChanged += (s,a) => RescaleUIToResolution(window);
        return this;
    }

    public void OnLoadRequest(LoadScreenEvent msg)
    {
        // perform cleanup before checking if a screen was passed
        CleanupLayer(msg.Layer);
        if (msg.ScreenFactory == null) return;

        // we have a screen to load (load it)
        LoadScreen(msg.ScreenFactory, msg.Layer);
    }


    public void CleanupLayer(ScreenLayer layer)
    {
        // cleanup old element of layer if already occupied
        if (_layers.TryGetValue(layer, out var oldScreen))
        {
            Gum.Root.Children.Remove(oldScreen);
            _layers.Remove(layer);
        }
    }

    public void LoadScreen(UIElementFactory factory, ScreenLayer layer, Action<GraphicalUiElement> callback = null)
    {
        CleanupLayer(layer);
        var newScreen = factory();
        newScreen.AddToRoot();
        newScreen.Z = (int) layer;

        // create new element and add to stuff
        _layers.Add(layer, newScreen);
        callback?.Invoke(newScreen);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var item in Gum.Root.Children)
        {
            if (item is InteractiveGue asInteractiveGue)
            {
                (asInteractiveGue.FormsControlAsObject as IUpdateScreen)?.Update(gameTime);
            }
        }
        Gum.Update(gameTime);
    }

    public void Draw()
    {
        Gum.Draw();
    }



    public void Dispose()
    {
        _eventBus.Unsubscribe<LoadScreenEvent>(OnLoadRequest);
        GC.SuppressFinalize(this);
    }
}
