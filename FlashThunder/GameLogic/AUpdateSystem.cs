using System;

namespace FlashThunder.GameLogic;

public abstract class AUpdateSystem<T> : IDisposable
{
    public abstract void Update(T parameter);
    
    public virtual void Dispose()
    {
        // Base implementation does nothing, subclasses can override
    }
}