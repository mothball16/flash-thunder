using FlashThunder.Enums;
using FlashThunder.Events;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using System;
using System.Collections.Generic;

namespace FlashThunder.Managers
{
    public sealed class UIManager : IDisposable
    {

        private static GumService Gum => GumService.Default;
        private readonly ContainerRuntime _root;
        private readonly Dictionary<ScreenLayer, GraphicalUiElement> _layers;

        public UIManager()
        {
            _root = new ContainerRuntime();
            _root.AddToManagers();
            _layers = [];

            EventBus.Subscribe<LoadScreenEvent>(OnLoadRequest);
        }

        public void OnLoadRequest(LoadScreenEvent msg)
        {
            
        }

        public void LoadScreen(Type name)
        {
            
        }


        public void Update(GameTime gameTime)
        {
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
