using fennecs;
using FlashThunder.Enums;
using FlashThunder.GameLogic.Actions;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic.Actions.Data;
using FlashThunder.GameLogic.Actions.Interfaces;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using System;
using System.Text.Json;

namespace FlashThunder.GameLogic._Shared.ComponentLoaders
{
    internal class SkillSetLoader : IComponentLoader
    {
        public void LoadComponent(Entity e, JsonElement rawData)
        {
            var skillSet = new SkillSet
            {
                Skills = []
            };

            foreach (JsonElement skill in rawData.EnumerateArray())
            {
                var name = skill.TryGetProperty("name", out var nameProp)
                    ? nameProp.GetString() : "Attack of Unknown Origin";
                var icon = skill.TryGetProperty("icon", out var iconProp)
                    ? iconProp.GetString() : "default_icon";
                var desc = skill.TryGetProperty("description", out var descProp)
                    ? descProp.GetString() : "Description of Unknown Origin";
                var cooldown = skill.TryGetProperty("cooldownBetweenTurns", out var cooldownProp)
                    ? cooldownProp.GetInt32() : 0;
                var usesPerTurn = skill.TryGetProperty("usesPerTurn", out var usesProp)
                    ? usesProp.GetInt32() : 1;
                var range = skill.TryGetProperty("range", out var rangeProp)
                    ? rangeProp.GetInt32() : 2;
                var traverse = skill.TryGetProperty("traverse", out var traverseProp)
                    ? JsonSerializer.Deserialize<string[]>(traverseProp.GetRawText(), options: DataLoader.Options) : ["land"];
                var selection = skill.TryGetProperty("selectionType", out var selectionProp)
                    ? Enum.Parse<SelectionType>(selectionProp.GetString(), ignoreCase: true) : SelectionType.Passthrough;
                var behavior = skill.TryGetProperty("attackBehavior", out var behaviorProp)
                    ? behaviorProp.GetString() : "BasicAttackBehavior";

                // nah this should fail if there isnt one
                var config = JsonSerializer.Deserialize<IAttackParams>(skill.GetProperty("attackParams").GetRawText(), options: DataLoader.Options);


                var unitSkill = new UnitSkill
                {
                    Name = name,
                    Icon = icon,
                    Description = desc,
                    CooldownBetweenTurns = cooldown,
                    UsesPerTurn = usesPerTurn,
                    Range = range,
                    Traverse = traverse,
                    SelectionType = selection,
                    AttackBehavior = behavior,
                    AttackParams = config
                };

                var skillState = new UnitSkillState { CanUse = true, TurnsSinceLastUse = 0, UsesLeftThisTurn = usesPerTurn};
                skillSet.Skills.Add(new SkillEntry { Data = unitSkill, State = skillState });
            }
            e.Add(skillSet);
        }
    }
}
