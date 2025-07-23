using fennecs;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.Utilities;
using System.Text.Json;

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
