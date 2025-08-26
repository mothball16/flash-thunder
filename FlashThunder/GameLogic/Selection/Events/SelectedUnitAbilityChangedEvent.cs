using FlashThunder.GameLogic.Actions.Components;

namespace FlashThunder.GameLogic.Selection.Events;

internal readonly record struct SelectedUnitAbilityChangedEvent(SkillSet SkillSet, int AbilityIndex);