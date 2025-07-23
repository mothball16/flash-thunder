using fennecs;
using FlashThunder.Defs;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlashThunder.ECSGameLogic.ComponentLoaders;

/// <summary>
/// Bundles some internal components with the addition of the moveCapable component.
/// </summary>
internal class MoveCapableLoader : IComponentLoader
{
    public void LoadComponent(Entity e, JsonElement rawData)
    {
        var component = DataLoader.DeserObject<MoveCapable>(rawData.GetRawText());
        e.Add(component);

        // add this too because this should always exist alongside moveCapable
        e.Add(new MoveIntent());
    }
}
