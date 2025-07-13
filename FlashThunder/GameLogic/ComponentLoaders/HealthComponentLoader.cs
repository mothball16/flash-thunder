using fennecs;
using FlashThunder.Defs;
using FlashThunder.ECSGameLogic.Components.UnitStats;
using FlashThunder.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlashThunder.ECSGameLogic.ComponentLoaders;

internal class HealthComponentLoader : IComponentLoader
{
    public void LoadComponent(Entity e, JsonElement rawData)
    {
        var max = rawData.GetProperty("maxHealth").GetInt32();
        var healthExists = rawData.TryGetProperty("health", out var health);
        var healthComponent = new Health()
        {
            MaxHealth = max,
            CurHealth = healthExists ? health.GetInt32() : max
        };
        e.Add(healthComponent);
    }
}
