using FlashThunder.Events;
using FlashThunder.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace FlashThunder.Managers
{
    public sealed class StateManager
    {
        private readonly Dictionary<Type, Func<IGameState>> _stateFacs;
        private IGameState _currentState;

        public StateManager(EventBus eventBus)
        {
            _stateFacs = [];
            eventBus.Subscribe<ChangeStateEvent>(OnStateChanged);
        }

        public void OnStateChanged(ChangeStateEvent msg)
        {
            SwitchTo(msg.To);
        }

        public StateManager Register(Type state, Func<IGameState> stateFac)
        {
            _stateFacs.Add(state, stateFac);

            return this;
        }

        /// <summary>
        /// Enter a new state, exiting the current one.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public StateManager SwitchTo(Type stateType)
        {
            if (_stateFacs.TryGetValue(stateType, out Func<IGameState> stateFac))
            {
                _currentState?.Exit();
                _currentState = stateFac();
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
