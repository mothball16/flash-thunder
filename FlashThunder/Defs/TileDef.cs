using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        bool Walkable {  get; set; }
        
    }
}
