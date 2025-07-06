using DefaultEcs;
using DefaultEcs.System;
using FlashThunder._ECSGameLogic;
using FlashThunder.ECSGameLogic.Resources;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.ECSGameLogic.Systems.OnDraw;

/// <summary>
/// Begins the spritebatch based off the cam
/// </summary>
internal sealed class SpriteBatchInitSystem : ISystem<DrawFrameSnapshot>
{
    // - - - [ Private Fields ] - - -
    private readonly World _world;

    // - - - [ Properties ] - - -
    public bool IsEnabled { get; set; }

    public SpriteBatchInitSystem(World world) 
        => _world = world;

    public void Update(DrawFrameSnapshot state)
    {
        state.SpriteBatch.Begin(SpriteSortMode.Deferred,
             BlendState.AlphaBlend,
             SamplerState.PointClamp,
             null, null, null,
             transformMatrix: _world.Get<CameraResource>().TransformMatrix);
    }

    public void Dispose() { }
}