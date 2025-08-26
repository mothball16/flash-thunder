using fennecs;
using FlashThunder.GameLogic.Actions.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Actions.Data;

internal record EmptyParams() : IAttackParams;
internal record DefaultAttackParams(int Damage, int RandomRange, int AP) : IAttackParams;
internal record ExplosiveAttackParams(int Damage, int RandomRange, int Tiles, int Decay) : IAttackParams;