using DefaultEcs;

namespace FlashThunder.ECSGameLogic.Components;

/// <summary>
/// Marks an entity as being able to be owned by a team. The owner is stored as an entity
/// (which should have the corresponding team components)
/// </summary>
internal struct OwnableComponent
{
    public Entity Owner { get; set; }
}
