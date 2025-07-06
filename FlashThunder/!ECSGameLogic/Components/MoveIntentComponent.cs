using FlashThunder.Enums;

namespace FlashThunder.ECSGameLogic.Components;

internal struct MoveIntentComponent
{
    public Direction Dir { get; set; }
    /// <summary>
    /// How many frames till the moveintent can be processed?
    /// </summary>
    public int Logistic { get; set; }
}