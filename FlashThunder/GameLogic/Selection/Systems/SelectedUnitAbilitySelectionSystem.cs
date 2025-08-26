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
            var input = _world.GetResource<InputResource>();

            // for each selected unit with skills where the ability input was pressed...
            _selectedWithSkills.For((in Entity e, ref SkillSet skillSet) =>
            {
                foreach (var ability in _abilityMap
                .Where(ability => input.WasJustActivated(ability.Key))
                .Select(ability => ability.Value))
                {
                    // if the unit doesn't have an ability selected, add and set AbilitySelected tag 
                    if (!e.Has<AbilitySelected>())
                    {
                        e.Add(new AbilitySelected { AbilityIndex = ability });
                        _uiNotifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, ability));
                    }
                    // if the ability selected is different from the one pressed, update AbilitySelected tag
                    else if (e.Ref<AbilitySelected>().AbilityIndex != ability)
                    {
                        e.Ref<AbilitySelected>().AbilityIndex = ability;
                        _uiNotifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, ability));
                    }
                    // if the ability selected is the same as the one pressed, remove AbilitySelected tag
                    else
                    {
                        e.Remove<AbilitySelected>();
                        _uiNotifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet,-1));
                    }

                    e.TryRemove<ActionTiles>();
                }
            });
        }

    }
}
