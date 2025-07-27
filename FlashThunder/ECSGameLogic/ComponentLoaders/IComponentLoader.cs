using fennecs;
using System.Text.Json;

namespace FlashThunder.ECSGameLogic.ComponentLoaders;

public interface IComponentLoader
{
    void LoadComponent(Entity entity, JsonElement rawData);
}