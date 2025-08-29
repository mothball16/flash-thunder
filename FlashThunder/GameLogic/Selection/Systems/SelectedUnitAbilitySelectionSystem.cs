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
    internal class SelectedUnitAbilitySelectionSystem : AUpdateSystem<float>
    {
        private readonly World _world;
        private readonly Stream<SkillSet> _selectedWithSkills;
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
        public SelectedUnitAbilitySelectionSystem(World world, IEventPublisher uiNotifier)
        {
            _world = world;
            _selectedWithSkills = world.Query<SkillSet>()
                .Has<SelectedTag>()
                .Stream();
            _uiNotifier = uiNotifier;
        }

        public override void Update(float upd)
        {
            // we need to know if any ability selection inputs have been activated
            var input = _world.GetResource<InputResource>();

            int? ability = null;
            foreach(var a in _abilityMap)
            {
                if (input.WasJustActivated(a.Key))
                {
                    ability = a.Value;
                    break;
                }
            }

            //no need to run the below logic if we didnt press anything
            if (ability is null)
                return;

            // for each selected unit with skills where the ability input was pressed...
            _selectedWithSkills.For(
                uniform: (_uiNotifier, upd, ability.Value),
                action: static ((IEventPublisher notifier, float upd, int ability) uniform,
                in Entity e, ref SkillSet skillSet) =>
            {
                // if the unit doesn't have an ability selected, add and set AbilitySelected tag 
                if (!e.Has<AbilitySelected>())
                {
                    e.Add(new AbilitySelected { AbilityIndex = uniform.ability });
                    uniform.notifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, uniform.ability));
                }
                // if the ability selected is different from the one pressed, update AbilitySelected tag
                else if (e.Ref<AbilitySelected>().AbilityIndex != uniform.ability)
                {
                    e.Ref<AbilitySelected>().AbilityIndex = uniform.ability;
                    uniform.notifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, uniform.ability));
                }
                // if the ability selected is the same as the one pressed, remove AbilitySelected tag
                else
                {
                    e.Remove<AbilitySelected>();
                    uniform.notifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet,-1));
                }
                // we should not have action tiles persist upon an ability change because they depend
                // on the ability -- this will cause actiontiles to recalculate when the tile update
                // system runs
                e.TryRemove<ActionTiles>();
            });
        }

    }
}
