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
    /// Defines a JSON entry into the TextureManager.
    /// </summary>
    public class TextureDef
    {
        public string TextureName { get; set; }
        public string TextureAlias { get; set; }
    }
}
