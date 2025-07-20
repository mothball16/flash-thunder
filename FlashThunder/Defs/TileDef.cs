using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace FlashThunder.Defs;

/// <summary>
/// Defines the general behavior of a tile.
/// This is only created once per tile-type -- any behaviors are to be dealt with
/// outside of the definition.
/// </summary>
internal class TileDef
{
    public string TextureName { get; set; }
    /// <summary>
    /// What type of traversal method can access this tile?
    /// </summary>
    public string[] Traverse { get; set; }

    /// <summary>
    /// How many moves does it take to traverse this tile?
    /// </summary>
    public float Cost { get; set; } = 1;

    /// <summary>
    /// Loaded during/after initialization, used to retrieve tile tex
    /// </summary>
    [JsonIgnore]
    public Texture2D Texture { get; set; }
}
