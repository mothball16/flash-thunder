using fennecs;
using FlashThunder.GameLogic.Attacks.Data;

namespace FlashThunder.GameLogic.Attacks.Interfaces;

public interface IAttackBehavior
{
    AttackInstance Execute(World world, AttackData data);
}