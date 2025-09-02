using fennecs;
using FlashThunder.Enums;
using FlashThunder.GameLogic._Shared;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.GameLogic.Selection.Events;
using FlashThunder.Managers;
using System.Collections.Generic;
using System.Linq;

namespace FlashThunder.GameLogic.Selection.Systems
{
    internal class AbilitySelectSystems : AUpdateSystem<float>
    {
        private readonly World _world;
        private readonly Stream<SkillSet> _selectedWithSkills;
        private readonly Stream<SkillSet, AbilitySelected> _selectedWithActiveSkill;
        private readonly IEventPublisher _uiNotifier;
        private readonly Dictionary<GameAction, int> _abilityMap = new()
        {
            {GameAction.Ability1, 0},
            {GameAction.Ability2, 1},
            {GameAction.Ability3, 2},
            {GameAction.Ability4, 3},
            {GameAction.Ability5, 4},
            {GameAction.Ability6, 5},
            {GameAction.Ability7, 6},
            {GameAction.Ability8, 7},
            {GameAction.Ability9, 8},
            {GameAction.Ability10, 9},
        };
        public AbilitySelectSystems(World world, IEventPublisher uiNotifier)
        {
            _world = world;
            _selectedWithSkills = world.Query<SkillSet>()
                .Has<SelectedTag>()
                .Stream();
            _selectedWithActiveSkill = world.Query<SkillSet, AbilitySelected>()
                .Has<SelectedTag>()
                .Stream();
            _uiNotifier = uiNotifier;
        }

        private static void DeselectAbility(Entity e, SkillSet skillSet, IEventPublisher notifier)
        {
            e.TryRemove<AbilitySelected>();
            e.TryRemove<ActionTiles>();
            notifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, -1));
        }

        private void SelectOnInputSystem(int ability, float upd)
        {
            // for each selected unit with skills where the ability input was pressed...
            _selectedWithSkills.For(
                uniform: (_uiNotifier, upd, ability),
                action: static ((IEventPublisher notifier, float upd, int ability) uniform,
                in Entity e, ref SkillSet skillSet) =>
                {
                    if (skillSet[uniform.ability].IsValid)
                    {
                        // if the unit doesn't have an ability selected, add and set AbilitySelected tag 
                        if (!e.Has<AbilitySelected>())
                        {
                            e.Add(new AbilitySelected { AbilityIndex = uniform.ability });
                            e.TryRemove<ActionTiles>();
                            uniform.notifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, uniform.ability));
                        }
                        // if the ability selected is different from the one pressed, update AbilitySelected tag
                        else if (e.Ref<AbilitySelected>().AbilityIndex != uniform.ability)
                        {
                            e.Ref<AbilitySelected>().AbilityIndex = uniform.ability;
                            e.TryRemove<ActionTiles>();
                            uniform.notifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, uniform.ability));
                        }
                        // if the ability selected is the same as the one pressed, remove AbilitySelected tag
                        else
                        {
                            DeselectAbility(e, skillSet, uniform.notifier);
                        }
                    }
                });
        }

        /// <summary>
        /// Deselects actions on units that selected an action they can't perform.
        /// </summary>
        private void ValidateActionSystem()
        {
            _selectedWithActiveSkill.For(
                uniform: _uiNotifier,
                action: static (IEventPublisher uniform, in Entity e, ref SkillSet skillSet, ref AbilitySelected ability) => {
                    if (!skillSet[ability.AbilityIndex].IsValid)
                        DeselectAbility(e, skillSet, uniform);
                });
        }

        public override void Update(float upd)
        {
            // we need to know if any ability selection inputs have been activated
            var input = _world.GetResource<InputResource>();

            int? ability = null;
            foreach (var a in _abilityMap)
            {
                if (input.WasJustActivated(a.Key))
                {
                    ability = a.Value;
                    break;
                }
            }
            if (ability is not null)
                SelectOnInputSystem(ability.Value, upd);

            ValidateActionSystem();
        }
    }
}
