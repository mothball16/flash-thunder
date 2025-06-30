using FlashThunder.Events;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.Interfaces
{
    public interface IGameState
    {
        public void Enter();
        public void Exit();
        public void Update(GameTime time)
            => Update((float) time.ElapsedGameTime.TotalSeconds);
        public void Update(float dt);
        public void Draw(SpriteBatch sb);
    }
}
