using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Core.Components
{
    /// <summary>
    /// Global component for rendering tiles.
    /// Consider making a map entity instead -- this is just to make sure it works for now
    /// </summary>
    public class TileMapComponent
    {
        public char[][] Map {  get; set; }
        }
}
