namespace FlashThunder.Core;

internal static class GameConstants
{
    // Path to the shared content folder so I don't need to update separate files for shared
    // Gum/MGCB usage.
    public const string SharedContentPath = "Content/Shared";

    public const int TileSize = 64;
    public const float MaxZoom = 8;
    public const float MinZoom = 0.5f;
    // The scroll value delta reqiured to go up one zoom level.
    public const float ScrollStep = 120 * 4;

    // The identifier for an ability to be used as the move action. This is placed here so that
    // if it ever changes (it really shouldn't), it doesn't need to be updated on every system
    // that checks for it.
    public const string MoveBehaviorTag = "MoveAbilityINTERNAL";
}