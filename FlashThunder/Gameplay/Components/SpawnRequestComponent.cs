using FlashThunder.Defs;
using FlashThunder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Components
{
    public struct SpawnRequestComponent
    {
        public string EntityID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
