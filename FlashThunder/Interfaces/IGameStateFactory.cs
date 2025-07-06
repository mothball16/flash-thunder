using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.Interfaces;

/// <summary>
/// The IGameStateFactory interface should be responsible for the creation of heavy-duty states.
/// We can just use lambdas otherwise.
/// </summary>
public interface IGameStateFactory
{
    public IGameState Create();
}
