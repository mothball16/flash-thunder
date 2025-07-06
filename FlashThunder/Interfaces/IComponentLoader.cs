using DefaultEcs;
using FlashThunder.Events;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text.Json;

namespace FlashThunder.Interfaces;

/// <summary>
/// Used to handle special cases of component loading where dependencies are required.
/// 
/// </summary>
public interface IComponentLoader
{
    public void LoadComponent(Entity e, JsonElement rawData);
}
