using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DefaultEcs;
using System.Text.Json;

namespace FlashThunder.Defs
{
    /// <summary>
    /// Contains the template for an entity.
    /// This cannot create an entity on its own. The EntityFactory processes this information
    /// and creates an entity with it.
    /// </summary>
    public class EntityTemplateDef
    {
        public Dictionary<string, JsonElement> Components { get; set; }
    }
}