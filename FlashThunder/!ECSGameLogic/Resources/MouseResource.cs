using Microsoft.Xna.Framework;

namespace FlashThunder.ECSGameLogic.Resources
{
    /// <summary>
    /// Global component for retrieving the mouse information during a frame.
    /// </summary>
    public struct MouseResource
    {
        public Point Diff { get; set; }
        public float Delta { get; set; }
        public Point Position { get; set; }
        public Point WorldPosition { get; set; }
        public Point TilePosition { get; set; }
        public bool LPressed { get; set; }
        public bool MPressed { get; set; }
        public bool RPressed { get; set; }
        public readonly int ScreenX
            => Position.X;
        public readonly int ScreenY
            => Position.Y;
        public readonly int WorldX
            => WorldPosition.X;
        public readonly int WorldY
            => WorldPosition.Y;
        public readonly int TileX
            => TilePosition.X;
        public readonly int TileY
            => TilePosition.Y;
    }
}
