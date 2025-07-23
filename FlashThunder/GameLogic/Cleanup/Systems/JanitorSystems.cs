using fennecs;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Extensions;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.Managers;
using FlashThunder.Utilities;
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
/// Systems that are run after the main cycle to reset/refresh anything we may need, usually
/// resources.
/// </summary>
/// <param name="world"></param>
internal sealed class JanitorSystems: IUpdateSystem<float>
{
    private readonly World _world;

    public JanitorSystems(World world)
    {
        _world = world;
    }

    public void Update(float dt)
    {
        InputRefreshSystem();
    }

    private void InputRefreshSystem()
    {
        var input = _world.GetResource<InputResource>();
        input.ConsumedInputs.Clear();
    }


    public void Dispose() { }
}
