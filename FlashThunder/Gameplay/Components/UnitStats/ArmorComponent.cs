using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Gameplay.Components.UnitStats
{
    /// <summary>
    /// Calculation for armor damage:
    /// hardFinal = Math.Max(hard - hardPen, 0);
    /// softFinal = Math.Max(soft / (softPen / 100), 0);
    /// afterArmorDmg = (Math.Max(dmg - hardFinal,0) * (1 - softFinal/100))
    /// </summary>
    public struct ArmorComponent 
    {
        /// <summary>
        /// The "hard" decrease in damage. 20 damage thru 10 hard armor -> 10.
        /// (20 - 10)
        /// </summary>
        public int Hard { get; set; }
        /// <summary>
        /// The "soft" percentage-based decrease in damage. 20 damage thru 10 soft armor -> 18.
        /// (20 * (1 - soft/100))
        /// </summary>
        public int Soft { get; set; }
        
    }
}
