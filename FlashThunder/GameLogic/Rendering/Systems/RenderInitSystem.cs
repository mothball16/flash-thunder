using Dcrew.MonoGame._2D_Camera;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.GameLogic.Rendering.Systems;

/// <summary>
/// Initializes the SpriteBatch for rendering based off the camera instance's transofrm matrix.
/// </summary>
/// <param name="camera"></param>
internal sealed class RenderInitSystem(Camera camera)
    : IUpdateSystem<SpriteBatch>
{
    private readonly Camera _camera = camera;
    public void Update(SpriteBatch sb)
    {
        sb.Begin(SpriteSortMode.Deferred,
             BlendState.AlphaBlend,
             SamplerState.PointClamp,
             null, null, null,
             transformMatrix: _camera.View(0));
    }
    public void Dispose() { }
}
