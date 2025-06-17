using DefaultEcs;
using FlashThunder.Defs;
using FlashThunder.Gameplay.Components;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using Microsoft.Xna.Framework.Graphics;
using FlashThunder.Extensions;
using FlashThunder.Gameplay.Components.UnitStats;

namespace FlashThunder.Core
{
    /// <summary>
    /// Spawns entities by entity ID.
    /// </summary>
    public class EntityFactory
    {
        private readonly World _world;
        private readonly TexManager _texManager;
        private readonly Dictionary<string, EntityTemplateDef> _templates;

        public EntityFactory(World world, TexManager textureManager)
        {
            _world = world;
            _texManager = textureManager;
            _templates = [];
        }

        private static string ToComponentKey(string raw)
            => string.Concat(raw.FirstCharToUpper(), "Component");

        public EntityFactory LoadTemplates(string filePath)
        {
            var templates = DataLoader.LoadObject<Dictionary<string, EntityTemplateDef>>(filePath);

            foreach (var template in templates)
            {
                if (_templates.ContainsKey(template.Key))
                {
                    Console.WriteLine($"[WARNING] Duplicated binding on {template.Key}");
                }

                _templates[template.Key] = template.Value;
            }

            return this;
        }

        public EntityFactory Clear()
        {
            _templates.Clear();
            return this;
        }

        public Entity CreateEntity(string id)
        {
            var entity = _world.CreateEntity();

            if (!_templates.TryGetValue(id, out EntityTemplateDef template))
                throw new KeyNotFoundException($"[ERROR] {id} was not found in the entity templates.");

            // Local helper for when we don't need any additional logic.
            void LoadDefault<T>(string data)
            {
                var component = DataLoader.DeserObject<T>(data);
                entity.Set<T>(component);
            }

            foreach (var componentRep in template.Components)
            {
                var rawData = componentRep.Value.GetRawText();
                var jsonData = componentRep.Value;
                var componentName = ToComponentKey(componentRep.Key);

                switch (componentName)
                {
                    // - - - [ components that get handled by default ] - - -
                    case nameof(VisionComponent): LoadDefault<VisionComponent>(rawData); break;
                    case nameof(ControlledComponent): LoadDefault<ControlledComponent>(rawData); break;
                    case nameof(GridPosComponent): LoadDefault<GridPosComponent>(rawData); break;
                    case nameof(ArmorComponent): LoadDefault<ArmorComponent>(rawData); break;
                    case nameof(TileMapComponent): LoadDefault<TileMapComponent>(rawData); break;
                    case nameof(MoveComponent): LoadDefault<MoveComponent>(rawData); break;

                    // - - - [ components w/ special handling ] - - -
                    case nameof(HealthComponent):
                        var max = jsonData.GetProperty("maxHealth").GetInt32();
                        var healthExists = jsonData.TryGetProperty("health", out var health);
                        var healthComponent = new HealthComponent()
                        {
                            MaxHealth = max,
                            Health = healthExists ? health.GetInt32() : max
                        };
                        entity.Set(healthComponent);
                        break;
                    case nameof(SpriteDataComponent):
                        var tex = _texManager
                            [jsonData.GetProperty("textureAlias").GetString()];

                        var scaleX = jsonData.GetProperty("sizeX").GetInt32();
                        var scaleY = jsonData.GetProperty("sizeY").GetInt32();

                        var component = new SpriteDataComponent(tex, scaleX, scaleY);
                        entity.Set(component);
                        break;

                    default: // - - - [ unhandled component (not good), throw exception ] - - -
                        Console.WriteLine(
                            $"[ERROR] Unhandled component {componentName} in entity template {id}");
                        break;

                }
            }

            return entity;
        }

        public Entity CreateEntity(string id, Point pos)
            => CreateEntity(id, pos.X, pos.Y);

        public Entity CreateEntity(string id, int x, int y)
        {
            var entity = CreateEntity(id);
            entity.Set<GridPosComponent>(new(x, y));
            return entity;
        }
    }
}