using fennecs;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using System.Text.Json;

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
