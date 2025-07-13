using fennecs;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        //full initialization
        foreach(var layer in rawData.GetProperty("layers").EnumerateObject())
        {
            initializedLayers.Add(layer.Name, LoadLayer(layer.Value));
        }
        e.Add(new SpriteData(initializedLayers));
    }
}
