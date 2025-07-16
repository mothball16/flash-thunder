using FlashThunder.Defs;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Components.UnitStats;
using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;
using FlashThunder._ECSGameLogic;
using System.Reflection;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using fennecs;
using FlashThunder.GameLogic;
using FlashThunder._ECSGameLogic.Components.TeamStats;

namespace FlashThunder.Factories;

public delegate void ComponentLoaderDelegate(Entity e, JsonElement data);

/// <summary>
/// Spawns entities by entity ID.
/// </summary>
internal class EntityFactory
{
    private readonly World _world;
    private readonly Dictionary<string, EntityTemplateDef> _templates;
    private readonly Dictionary<string, ComponentLoaderDelegate> _loaders;

    public EntityFactory(World world)
    {
        _world = world;
        _templates = [];
        _loaders = [];
    }

    private static string ToComponentKey<T>()
        => typeof(T).Name.FirstCharToLower();

    /// <summary>
    /// Maps a component type to the DEFAULT loader. Can be overridden by AddCustomLoader.
    /// </summary>
    public EntityFactory Map<T>(IComponentLoader loader = null)
    {
        // get key and check for any bad practices in usage
        var key = ToComponentKey<T>();
        if (_loaders.ContainsKey(key))
            Logger.Warn($"Loader on {key} was re-assigned. Loaders should not be re-assigned after creation.");

        // add the new typed loader to the loader dictionary
        _loaders[typeof(T).Name.FirstCharToLower()]
            // do we have a custom loader?
            = loader == null
            // (no) load default
            ? (e, data) =>
            {
                var component = DataLoader.DeserObject<T>(data.GetRawText());
                e.Add(component);
            }
            // (yes) load custom
            : (e, data) => loader.LoadComponent(e, data);
        return this;
    }

    public EntityFactory LoadTemplates(string filePath)
    {
        var templates = DataLoader.LoadObject<Dictionary<string, EntityTemplateDef>>(filePath);

        foreach (var template in templates)
        {
            if (_templates.ContainsKey(template.Key))
            {
                Logger.Warn($"Duplicated binding on {template.Key}");
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

    public Entity CreatePrefab(string id)
    {
        Logger.Print("!! beginning entity creation...");
        var entity = _world.Spawn();
        if (!_templates.TryGetValue(id, out EntityTemplateDef template))
            throw new KeyNotFoundException($"[ERROR] {id} was not found in the entity templates.");

        foreach (var componentRep in template.Components)
        {
            var cmpName = componentRep.Key;
            var cmpData = componentRep.Value;
            if (!_loaders.TryGetValue(cmpName, out ComponentLoaderDelegate loader))
            {
                Logger.Warn($"No loader found for {cmpName}");
                continue;
            }
            Logger.Print($"loading {cmpName} with value {cmpData}...");
            loader(entity, cmpData);
        }
        Logger.Confirm("entity loaded!");
        return entity;
    }

    #region - - - [ Bundles ] - - -

    public Entity CreateTeamBundle(
        TeamTag teamTag,
        Faction faction,
        IsPlayerControllable? playerControllable = null)
    {
        var entity = _world.Spawn()
            .Add(teamTag)
            .Add(faction);
        if (playerControllable.HasValue)
            entity.Add(playerControllable.Value);
        return entity;
    }

    #endregion - - - [ Bundles ] - - -
}