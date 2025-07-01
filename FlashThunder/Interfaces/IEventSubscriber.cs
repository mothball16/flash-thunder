using FlashThunder.Events;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlashThunder.Interfaces
{
    public interface IEventSubscriber
    {
        public void Subscribe<T>(Action<T> handler);
        public void Unsubscribe<T>(Action<T> handler);
    }
}
