using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Selection.Components
{
    internal struct AbilitySelected
    {
        public int AbilityIndex { get; set; }

        public AbilitySelected()
        {
            AbilityIndex = -1;
        }
    }
}
