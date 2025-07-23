using fennecs;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Rendering.Components;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using System.Collections.Generic;
using System.Text.Json;

namespace FlashThunder.ECSGameLogic.ComponentLoaders;

internal class SpriteDataComponentLoader : IComponentLoader
{
    private readonly TextureManager _texManager;
    public SpriteDataComponentLoader(TextureManager texManager)
    {
        _texManager = texManager;
    }
    public void LoadComponent(Entity e, JsonElement rawData)
    {
        Dictionary<string, SpriteLayer> initializedLayers = [];
        SpriteLayer LoadLayer(JsonElement raw)
        {
            var tex = _texManager[raw.GetProperty("textureAlias").GetString()];
            var scaleX = raw.GetProperty("sizeX").GetInt32();
            var scaleY = raw.GetProperty("sizeY").GetInt32();
            return new SpriteLayer
            {
                Texture = tex,
                SizeX = scaleX,
                SizeY = scaleY
            };
        }
        Logger.Error($"[DEBUG] {rawData.GetProperty("layers").EnumerateObject()}");
        //full initialization
        foreach(var layer in rawData.GetProperty("layers").EnumerateObject())
        {

            initializedLayers.Add(layer.Name, LoadLayer(layer.Value));
        }
        e.Add(new SpriteData(initializedLayers));
    }
}
