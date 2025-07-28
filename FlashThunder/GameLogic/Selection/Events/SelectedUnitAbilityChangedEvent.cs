using FlashThunder.GameLogic.Attacks.Components;

namespace FlashThunder.GameLogic.Selection.Events;

internal readonly record struct SelectedUnitAbilityChangedEvent(SkillSet SkillSet, int AbilityIndex);