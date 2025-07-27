using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlashThunder.States;

internal interface IGameState : IDisposable
{
    void Enter();
    void Exit();
    void Update(float dt);
    void Draw(SpriteBatch sb);
}