using fennecs;
using FlashThunder.Defs;
using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Managers;
using FlashThunder.Screens.Handlers;
using FlashThunder.Utilities;
using Gum.DataTypes;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlashThunder.Screens.Management;

internal sealed class ScreenManager : IDisposable
{
    private static Point OriginalUIDimensions = new(1920, 1080);
    private static GumService Gum => GumService.Default;

    private readonly GumProjectSave _project;

    private readonly Dictionary<ScreenLayer, GraphicalUiElement> _layers;
    private readonly List<IDisposable> _disposables;
    private readonly ScreenFactory _factory;

    public ScreenManager(Game game, ScreenFactory factory)
    {
        _project = Gum.Initialize(game, AssetPaths.UIProj);
        _layers = [];
        _factory = factory;
        _disposables = [];
    }



    public ScreenManager RescaleUIToResolution(GameWindow window)
    {
        window.AllowUserResizing = true;
        var zoom = window.ClientBounds.Height / (float)OriginalUIDimensions.Y;
        Gum.Renderer.Camera.Zoom = zoom;
        GraphicalUiElement.CanvasWidth = OriginalUIDimensions.X / zoom;
        GraphicalUiElement.CanvasHeight = OriginalUIDimensions.Y / zoom;
        return this;
    }

    public ScreenManager SetupListeners(GameWindow window)
    {
        window.ClientSizeChanged += (s, a) => RescaleUIToResolution(window);
        return this;
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

    public void TransitionScreen(GraphicalUiElement newScreen, ScreenLayer layer)
    {
        CleanupLayer(layer);
        newScreen.AddToRoot();
        newScreen.Z = (int)layer;
        _layers.Add(layer, newScreen);
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
        _disposables.ForEach(d => d.Dispose());
        GC.SuppressFinalize(this);
    }



    #region - - - [ screen loading ] - - -
    public void LoadTitleScreen()
        => TransitionScreen(_factory.CreateTitleScreen(),ScreenLayer.Primary);

    public void LoadGameScreen(World world)
        => TransitionScreen(_factory.CreateGameScreen(world), ScreenLayer.Primary);
    #endregion
}
