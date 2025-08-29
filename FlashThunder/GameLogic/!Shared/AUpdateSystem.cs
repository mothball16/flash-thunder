using System;
using System.Collections.Generic;

namespace FlashThunder.GameLogic;

public abstract class AUpdateSystem<T> : IDisposable
{
    private bool _disposed = false;
    protected readonly List<IDisposable> _disposables = [];
    public abstract void Update(T upd);

    // still wrapping my head around this dispose pattern, but heres a placeholder so sonaranalyzer
    // stops screaming at me
    protected virtual void Dispose(bool disposing)
    {
        if(_disposed)
            return;
        // disposes of managed resources
        if (disposing)
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
