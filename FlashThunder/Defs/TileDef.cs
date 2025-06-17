using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlashThunder.Defs
{
    /// <summary>
    /// Defines the general behavior of a tile.
    /// This is only created once per tile-type -- any behaviors are to be dealt with
    /// outside of the definition.
    /// </summary>
    public class TileDef
    {
        public string TextureName { get; set; }
        /// <summary>
        /// Can land-travelling units pass?
        /// </summary>
        public bool ByLand { get; set; } = true;

        /// <summary>
        /// Can air-travelling units pass?
        /// </summary>
        public bool ByAir { get; set; } = true;

        /// <summary>
        /// Can sea-travelling units pass?
        /// </summary>
        public bool BySea { get; set; } = false;

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
}
