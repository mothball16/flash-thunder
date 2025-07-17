using FlashThunder.Enums;
using FlashThunder.GameLogic;

namespace FlashThunder.ECSGameLogic.Components;

internal struct MoveIntent
{
    public Direction Dir { get; set; }
    /// <summary>
    /// How many frames till the moveintent can be processed?
    /// </summary>
    public int Logistic { get; set; }
}