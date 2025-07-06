namespace FlashThunder.ECSGameLogic.Components;

internal struct ToDestroyComponent
{
}
internal struct ToDestroyInFramesComponent
{
    public int Lifetime { get; set; }
}
internal struct ToDestroyInSecondsComponent
{
    public float Lifetime { get; set; }
}
