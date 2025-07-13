using Dcrew.MonoGame._2D_Camera;
using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.GameLogic;
using FlashThunder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FlashThunder.GameLogic.Systems.OnDraw;

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
