using fennecs;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.Utilities;
using System.Text.Json;

namespace FlashThunder.ECSGameLogic.ComponentLoaders;

/// <summary>
/// Bundles some internal components with the addition of the moveCapable component.
/// </summary>
internal class GridMoverLoader : IComponentLoader
{
    public void LoadComponent(Entity e, JsonElement rawData)
    {
        var component = DataLoader.DeserObject<GridMover>(rawData.GetRawText());
        e.Add(component);

        // if we have a grid mover, then we need a move intent
        // (this is the whole point of the loader lol)
        e.Add(new MoveIntent());
    }
}
