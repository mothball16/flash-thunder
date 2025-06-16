using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashThunder.Enums;
using Microsoft.Xna.Framework;

namespace FlashThunder.Gameplay.Resources
{
    /// <summary>
    /// Global component for retrieving the active actions during a frame.
    /// </summary>
    public struct ActionsResource
    {
        public HashSet<GameAction> Active { get; set; }

        public ActionsResource()
        {
            Active = [];
        }

    }
}
