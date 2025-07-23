using FlashThunder.GameLogic.Attacks.Interfaces;

namespace FlashThunder.GameLogic.Attacks.Data;

internal record DefaultAttackParams(int Damage, int RandomRange, int AP) : IAttackParams;

internal record ExplosiveAttackParams(int Damage, int RandomRange, int Tiles, int Decay) : IAttackParams;