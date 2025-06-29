using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using Gum.DataTypes;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameGum.Forms;
using MonoGameGum.GueDeriving;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashThunder.Managers
{
    public delegate GraphicalUiElement UIElementFactory();
    public sealed class UIManager : IDisposable
    {

        private static GumService Gum => GumService.Default;
        private readonly Dictionary<ScreenLayer, GraphicalUiElement> _layers;
        public UIManager()
        {
            _layers = [];
            EventBus.Subscribe<LoadScreenEvent>(OnLoadRequest);
        }

        public UIManager SetupListeners(GameWindow window)
        {
            window.ClientSizeChanged += (s,a) =>
            {
                GraphicalUiElement.CanvasWidth = window.ClientBounds.Width;
                GraphicalUiElement.CanvasHeight = window.ClientBounds.Height;
            };
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

        public void LoadScreen(UIElementFactory screenFac, ScreenLayer layer)
        {
            CleanupLayer(layer);

            // the caller is responsible for dependencies here
            var newScreen = screenFac();
            newScreen.AddToRoot();
            newScreen.Z = (int) layer;


            // create new element and add to stuff
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
            EventBus.Unsubscribe<LoadScreenEvent>(OnLoadRequest);
            GC.SuppressFinalize(this);
        }
    }
}
