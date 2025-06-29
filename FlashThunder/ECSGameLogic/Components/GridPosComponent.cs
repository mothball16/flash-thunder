namespace FlashThunder.ECSGameLogic.Components
{
    public struct GridPosComponent
    {
        public int X { get; set; }
        public int Y { get; set; }

        public GridPosComponent(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
