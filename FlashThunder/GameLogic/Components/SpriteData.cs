using FlashThunder.GameLogic.Components;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlashThunder.ECSGameLogic.Components;

/// <summary>
/// Represents a collection of sprite layers for rendering. The ZIndex of each SpriteLayer
/// determines what is rendered over the other.
/// </summary>
internal class SpriteData
{
    public Dictionary<string, SpriteLayer> Layers { get; set; }
    public SpriteData(Dictionary<string, SpriteLayer> layers)
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

internal struct SpriteLayer
{
    public Texture2D Texture { get; set; }

    // scale as 1, 1 means that the sprite will be contained within a 1 x 1 tile
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    /// <summary>
    /// higher = rendered on top of lower z-index sprites
    /// </summary>
    public int ZIndex { get; set; }
}
