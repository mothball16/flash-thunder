using DefaultEcs;
using FlashThunder._ECSGameLogic.Misc;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlashThunder._ECSGameLogic.ComponentLoaders
{
    internal class SpriteDataComponentLoader : IComponentLoader
    {
        private readonly TexManager _texManager;
        public SpriteDataComponentLoader(TexManager texManager)
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
            e.Set(new SpriteDataComponent(initializedLayers));
        }
    }
}
