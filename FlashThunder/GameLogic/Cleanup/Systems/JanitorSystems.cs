using fennecs;
using FlashThunder.GameLogic.Input.Resources;

namespace FlashThunder.GameLogic.Cleanup.Systems;

/// <summary>
/// Systems that are run after the main cycle to reset/refresh anything we may need, usually
/// resources.
/// </summary>
internal sealed class JanitorSystems: AUpdateSystem<float>
{
    private readonly World _world;

    public JanitorSystems(World world)
    {
        _world = world;
    }

    public override void Update(float dt)
    {
        InputRefreshSystem();
    }

    private void InputRefreshSystem()
    {
        var input = _world.GetResource<InputResource>();
        input.ConsumedInputs.Clear();
    }
}
