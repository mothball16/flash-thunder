using FlashThunder.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace FlashThunder.Managers
{
    public sealed class StateManager
    {
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentState;

        public StateManager()
        {
            _states = [];
        }

        public StateManager Register(IGameState state)
        {
            _states.Add(state.GetType(), state);
            return this;
        }

        public StateManager SwitchTo(Type stateType)
        {
            if (_states.TryGetValue(stateType, out IGameState state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState?.Enter();
            } else
            {
                throw new InvalidOperationException($"state {stateType.Name} is not registered");
            }
            return this;
        }

        public void Update(float dt)
        {
            _currentState?.Update(dt);
        }

        public void Draw(SpriteBatch sb)
        {
            _currentState?.Draw(sb);
        }
    }
}
