using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;
using FlashThunder.Managers;


namespace FlashThunder.Core
{
    public class GameContext : IDisposable
    {
        // - - - [ Private Fields ] - - -


        //inputbridge only needs to be disposed of with GameContext. nothing should rly be
        //interacting with it outside of Dispose
        private InputBridge _inputBridge;
        private SequentialSystem<float> _updateSystems;
        private SequentialSystem<SpriteBatch> _drawSystems;
        
        // - - - [ Properties ] - - -
        public World World { get; init; }

        //hold a ref. to assetmanager, but this shouldn't really be changed by GameContext
        public TexManager AssetManager { get; }

        public GameContext(
            World world, 
            TexManager assetManager,
            InputBridge inputBridge, 
            SequentialSystem<float> onUpd, SequentialSystem<SpriteBatch> onDraw)
        {

            //set properties
            World = world;
            AssetManager = assetManager;

            //set priv fields
            _inputBridge = inputBridge;
            _updateSystems = onUpd;
            _drawSystems = onDraw;
        }

        public void Update(float dt)
        {
            _updateSystems.Update(dt);
        }

        public void Draw(SpriteBatch sb)
        {
            _drawSystems.Update(sb);
        }

        /// <summary>
        /// Nukes the GameContext. Should properly cleanup any game-runtime specific systems/objects.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            World.Dispose();

            _inputBridge?.Dispose();
            _updateSystems?.Dispose();
            _drawSystems?.Dispose();
        }
    }
}
