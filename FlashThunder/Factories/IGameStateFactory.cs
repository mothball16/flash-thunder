using FlashThunder.States;

namespace FlashThunder.Factories;

internal interface IGameStateFactory
{
    IGameState Create();
}