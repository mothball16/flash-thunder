using System.Collections.Generic;
using System.Text.Json;

namespace FlashThunder.Defs;

/// <summary>
/// Contains the template for an entity.
/// This cannot create an entity on its own. The EntityFactory processes this information
/// and creates an entity with it.
/// </summary>
internal class EntityTemplateDef
{
    public Dictionary<string, JsonElement> Components { get; set; }
}