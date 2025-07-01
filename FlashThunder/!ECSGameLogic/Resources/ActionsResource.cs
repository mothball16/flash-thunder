using System.Collections.Generic;
using FlashThunder.Enums;

namespace FlashThunder.ECSGameLogic.Resources
{
    /// <summary>
    /// Global component for retrieving the active actions during a frame.
    /// </summary>
    public struct ActionsResource
    {
        public HashSet<GameAction> Active { get; set; }

        public ActionsResource() => Active = [];
    }
}