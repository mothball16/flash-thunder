using fennecs;
using FlashThunder.GameLogic.Attacks.Data;

namespace FlashThunder.GameLogic.Attacks.Components;

/// <summary>
/// Component indicating an entity wants to attack. Contains the attack data to execute.
/// </summary>
internal readonly record struct WantsToAttack(AttackData AttackData);