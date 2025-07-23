using fennecs;
using FlashThunder.GameLogic.Input.Resources;

namespace FlashThunder.GameLogic.Cleanup.Systems;

/// <summary>
/// Systems that are run after the main cycle to reset/refresh anything we may need, usually
/// resources.
/// </summary>
/// <param name="world"></param>
internal sealed class JanitorSystems: IUpdateSystem<float>
{
    private readonly World _world;

    public JanitorSystems(World world)
    {
        _world = world;
    }

    public void Update(float dt)
    {
        InputRefreshSystem();
    }

    private void InputRefreshSystem()
    {
        var input = _world.GetResource<InputResource>();
        input.ConsumedInputs.Clear();
    }


    public void Dispose() { }
}
