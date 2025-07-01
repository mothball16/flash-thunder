using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using FlashThunder.Managers;
using FlashThunder.Factories;

namespace FlashThunder.Core
{
    public sealed class GameContext : IDisposable
    {
        // - - - [ Private Fields ] - - -
        // inputbridge only needs to be disposed of with GameContext. nothing should rly be
        // interacting with it outside of Dispose
        private readonly InputMediator _inputBridge;
        // (systems shouldn't know about each other anyways)
        private readonly SequentialSystem<float> _updateSystems;
        private readonly SequentialSystem<SpriteBatch> _drawSystems;

        // - - - [ Public Properties ] - - -
        public World World { get; }
        public EntityFactory Factory { get; }
        public TexManager AssetManager { get; }

        public GameContext(
            World world,
            EntityFactory factory,
            TexManager assetManager,
            InputMediator inputBridge,
            SequentialSystem<float> onUpd,
            SequentialSystem<SpriteBatch> onDraw)
        {
            // set properties
            World = world;
            Factory = factory;
            AssetManager = assetManager;

            // set priv fields
            _inputBridge = inputBridge;
            _updateSystems = onUpd;
            _drawSystems = onDraw;
        }

        public void Update(float dt) => _updateSystems.Update(dt);

        public void Draw(SpriteBatch sb) => _drawSystems.Update(sb);

        /// <summary>
        /// Nukes the GameContext. Should properly cleanup any game-runtime specific systems/objects.
        /// </summary>
        public void Dispose()
        {
            World.Dispose();

            _inputBridge?.Dispose();
            _updateSystems?.Dispose();
            _drawSystems?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}