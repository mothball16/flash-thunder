using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Graphics;


namespace FlashThunder
{
    public class GameContext : IDisposable
    {
        // - - - [ Private Fields ] - - -

        //inputbridge only needs to be disposed of with GameContext. nothing should rly be
        //interacting with it
        private InputBridge _inputBridge;

        private SequentialSystem<float> _updateSystems;
        private SequentialSystem<SpriteBatch> _drawSystems;
        
        // - - - [ Properties ] - - -
        public World World { get; init; }

        public GameContext(
            World world, 
            InputBridge inputBridge, 
            SequentialSystem<float> onUpd, SequentialSystem<SpriteBatch> onDraw)
        {

            //set properties
            World = world;

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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _inputBridge?.Dispose();
        }
    }
}
