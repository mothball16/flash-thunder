using DefaultEcs;
using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Extensions
{
    internal abstract class AStandardSystem<T> : ISystem<T>
    {
        public World World { get; set; }
        private bool _isDisposed;
        public bool IsEnabled { get; set; }

        protected AStandardSystem(World world)
        {
            _isDisposed = false;
            World = world;
        }
        public abstract void Update(T state);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(_isDisposed)
            {
                return;
            }
            if (disposing)
            {
                // dispose managed resources here
            }
            _isDisposed = true;
        }
    }
}
