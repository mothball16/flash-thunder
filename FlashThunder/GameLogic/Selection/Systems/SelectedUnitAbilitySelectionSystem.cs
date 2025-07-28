using fennecs;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Attacks.Components;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.GameLogic.Selection.Events;
using FlashThunder.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic.Selection.Systems
{
    internal class SelectedUnitAbilitySelectionSystem : AUpdateSystem<float>
    {
        private readonly World _world;
        private readonly Stream<SkillSet> _selectedWithSkills;
        private readonly IEventPublisher _uiNotifier;
        private readonly Dictionary<GameAction, int> _abilityMap = new()
        {
            {GameAction.Ability1, 0 },
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
            _selectedWithSkills.For((in Entity e, ref SkillSet skillSet) =>
            {
                foreach (var ability in _abilityMap
                .Where(ability => input.WasJustActivated(ability.Key))
                .Select(ability => ability.Value))
                {
                    if (!e.Has<AbilitySelected>())
                    {
                        e.Add(new AbilitySelected { AbilityIndex = ability });
                        _uiNotifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, ability));
                    }
                    else if (e.Ref<AbilitySelected>().AbilityIndex != ability)
                    {
                        e.Ref<AbilitySelected>().AbilityIndex = ability;
                        _uiNotifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet, ability));
                    }
                    else
                    {
                        e.Remove<AbilitySelected>();
                        _uiNotifier.Publish(new SelectedUnitAbilityChangedEvent(skillSet,-1));
                    }
                }
            });
        }

    }
}
