namespace FlashThunder.GameLogic.Components;
/// <summary>
/// This component provides the response factor that the WorldPosition (on-screen) should drift
/// towards the GridPosition (authoritative) with.
/// This component shouldn't be used without WorldPosition and GridPosition
/// </summary>
/// <param name="Response"></param>
internal readonly record struct WorldToGridMover(float Response);
