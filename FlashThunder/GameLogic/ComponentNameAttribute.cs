using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic
{
    /// <summary>
    /// marks a component with a name so if i ever rename one it doesnt brick my game
    /// </summary>
    /// <param name="name"></param>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    internal sealed class ComponentNameAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
