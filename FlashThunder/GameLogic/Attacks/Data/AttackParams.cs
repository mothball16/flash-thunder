using FlashThunder.GameLogic.AttackLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.AttackLogic.Data;

internal record DefaultAttackParams(int Damage, int RandomRange, int AP) : IAttackParams;

internal record ExplosiveAttackParams(int Damage, int RandomRange, int Tiles, int Decay) : IAttackParams;