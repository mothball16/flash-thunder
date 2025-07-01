using FlashThunder._ECSGameLogic.Misc;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlashThunder.ECSGameLogic.Components
{
    /// <summary>
    /// Represents a collection of sprite layers for rendering. The ZIndex of each SpriteLayer
    /// determines what is rendered over the other.
    /// </summary>
    public class SpriteDataComponent
    {
        public Dictionary<string, SpriteLayer> Layers { get; set; }
        public SpriteDataComponent(Dictionary<string, SpriteLayer> layers)
        {
            Layers = layers;
        }

        // shortcut methods
        public void AddLayer(string layerName, SpriteLayer layer)
            => Layers.Add(layerName, layer);

        public bool TryGetLayer(string layerName, out SpriteLayer layer)
            => Layers.TryGetValue(layerName, out layer);

        public void RemoveLayer(string layerName)
            => Layers.Remove(layerName);
    }
}