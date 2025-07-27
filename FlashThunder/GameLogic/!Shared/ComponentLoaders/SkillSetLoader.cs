using fennecs;
using FlashThunder.GameLogic.Attacks;
using FlashThunder.GameLogic.Attacks.Components;
using FlashThunder.GameLogic.Attacks.Data;
using FlashThunder.GameLogic.Attacks.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlashThunder.GameLogic._Shared.ComponentLoaders
{
    internal class SkillSetLoader : IComponentLoader
    {
        private readonly AttackManager _attackManager;
        private readonly TextureManager _textureManager;
        public SkillSetLoader(AttackManager attackManager, TextureManager textureManager)
        {
            _attackManager = attackManager;
            _textureManager = textureManager;
        }
        public void LoadComponent(Entity e, JsonElement rawData)
        {
            var skillSet = new SkillSet();

            foreach(JsonElement skill in rawData.EnumerateArray())
            {
                var name = skill.TryGetProperty("Name", out var nameProp) ? nameProp.GetString() : "Attack of Unknown Origin";
                var desc = skill.TryGetProperty("Description", out var descProp) ? descProp.GetString() : "Description of Unknown Origin";
                var icon = skill.TryGetProperty("Icon", out var iconProp) ? iconProp.GetString() : "default_icon";
                var cooldown = skill.TryGetProperty("Cooldown", out var cooldownProp) ? cooldownProp.GetInt32() : 0;
                var behavior = skill.GetProperty("AttackBehavior").GetString();

                var param = skill.GetProperty("AttackParams");


                Logger.Error("This has not been implemented yet! Look into polymorphic deserialization solutions first");

                skillSet.Skills.Add(new UnitSkill
                {
                    Name = name,
                    Description = desc,
                    IconTexture = _textureManager.Get(icon),
                    Cooldown = cooldown,
                    AttackBehavior = behavior,
                    AttackParams = default
                });
            }
        }
    }
}
