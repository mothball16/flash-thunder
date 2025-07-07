using DefaultEcs;

namespace FlashThunder.ECSGameLogic.Components;

internal struct SpawnRequestComponent
{
    public string EntityID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Entity Owner { get; set; }

    public SpawnRequestComponent(string entityID, int x, int y, Entity owner)
    {
        EntityID = entityID;
        X = x;
        Y = y;
        Owner = owner;
    }
}
