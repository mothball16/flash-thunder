using DefaultEcs;
using FlashThunder.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder._ECSGameLogic
{
    internal readonly record struct GameFrameSnapshot
    (
        float DeltaTime,
        World World,
        ActionSnapshot Actions,
        MouseSnapshot Mouse
    );
}
