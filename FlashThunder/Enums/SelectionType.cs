using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Enums;

public enum SelectionType
{
    Pathfinding,
    LineOfSight,
    Passthrough,
    Self,
    Friendlies,
    Enemies
}