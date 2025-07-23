using Microsoft.Xna.Framework;

namespace FlashThunder.GameLogic.CameraControl.Components;

internal class WorldCamera
{
    /// <summary>
    /// The current position of the camera.
    /// </summary>
    public Vector2 Position { get; set; } = Vector2.Zero;
    /// <summary>
    /// The goal of the camera.
    /// </summary>
    public Vector2 Target { get; set; } = Vector2.Zero;
    /// <summary>
    /// A persistent offset from the target.
    /// </summary>
    public Vector2 Offset { get; set; } = Vector2.Zero;
    /// <summary>
    /// An offset that is reset every frame.
    /// </summary>
    public Vector2 Jitter { get; set; } = Vector2.Zero;
    public float Response { get; set; } = 0.5f;
    public float Scale { get; set; }
}
