using DefaultEcs;
using FlashThunder.Snapshots;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder._ECSGameLogic
{
    internal readonly record struct DrawFrameSnapshot
    (
        SpriteBatch SpriteBatch,
        ActionSnapshot Actions,
        MouseSnapshot Mouse
    );
}
