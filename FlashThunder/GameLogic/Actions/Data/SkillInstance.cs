using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Actions.Data
{
    /// <summary>
    /// This is used to represent the 'live' instance of a skill on a unit. UnitSkill is the definition
    /// and UnitSkillState is the state
    /// </summary>
    public class UnitSkillState
    {
        public int UsesLeftThisTurn { get; set; }
        public int TurnsSinceLastUse { get; set; }
        public bool CanUse { get; set; }
    }
}
