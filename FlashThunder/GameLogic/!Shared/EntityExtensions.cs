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

        public static ref C RefOrAdd<C>(this Entity e) where C : new()
        {
            if (e.Has<C>())
                return ref e.Ref<C>();
            else
                return ref e.Add(new C()).Ref<C>();
        }
    }
}
