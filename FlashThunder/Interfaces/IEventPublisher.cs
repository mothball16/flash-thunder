using FlashThunder.Events;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlashThunder.Interfaces;

public interface IEventPublisher
{
    public void Publish<T>(T data);
}
