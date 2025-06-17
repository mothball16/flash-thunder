using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashThunder.Enums;

namespace FlashThunder.Gameplay.Components
{
    public struct MoveIntentComponent
    {
        public Direction Dir { get; set; }
        /// <summary>
        /// How many frames till the moveintent can be processed?
        /// </summary>
        public int Logistic { get; set; }
    }
}