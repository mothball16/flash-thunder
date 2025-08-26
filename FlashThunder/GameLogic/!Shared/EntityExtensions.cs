using fennecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic._Shared
{
    public static class EntityExtensions
    {
        public static bool TryRemove<C>(this Entity e)
        {
            if (e.Has<C>())
            {
                e.Remove<C>();
                return true;
            }
            return false;
        }
    }
}
