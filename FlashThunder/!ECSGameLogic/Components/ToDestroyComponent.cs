namespace FlashThunder.ECSGameLogic.Components
{
    public struct ToDestroyComponent
    {
    }
    public struct ToDestroyInFramesComponent
    {
        public int Lifetime { get; set; }
    }
    public struct ToDestroyInSecondsComponent
    {
        public float Lifetime { get; set; }
    }
}
