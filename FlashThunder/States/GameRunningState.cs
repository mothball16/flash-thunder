using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Systems.OnDraw;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Bridges;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Debugging;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Reactive;
using FlashThunder.Enums;
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.States
{
    /// <summary>
    /// note -- The context should never exist outside of GameRunningState's lifecycle
    /// </summary>
    public class GameRunningState : IGameState
    {
        private readonly GameContext _context;
        private readonly EventBus _eventBus;

        public GameRunningState(
            EventBus eventBus,
            GameContext context)
        {
            _eventBus = eventBus;
            _context = context;
        }

        public void Enter()
        {
            _eventBus.Publish<LoadScreenEvent>(new()
            {
                ScreenFactory = () => new GameScreen().Visual,
                Layer = ScreenLayer.Primary
            });
        }
        public void Exit()
        {
            _context?.Dispose();
        }
        public void Update(float dt)
        {
            _context.Update(dt);
        }
        public void Draw(SpriteBatch sb)
        {

            _context.Draw(sb);
        }
    }
}
