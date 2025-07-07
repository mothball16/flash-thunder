using DefaultEcs;
using FlashThunder.Defs;
using FlashThunder.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FlashThunder.Managers;
using FlashThunder.Extensions;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Components.UnitStats;
using FlashThunder._ECSGameLogic.Misc;
using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;
using FlashThunder._ECSGameLogic;
using FlashThunder.Interfaces;
using System.Reflection;
using System.Linq;
using DefaultEcs.System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace FlashThunder.Factories;

public delegate void ComponentLoaderDelegate(Entity e, JsonElement data);

/// <summary>
/// Spawns entities by entity ID.
/// </summary>
internal class EntityFactory
{
    private const string ComponentIdentifier = "Component";
    private readonly World _world;
    private readonly Dictionary<string, EntityTemplateDef> _templates;
    private readonly Dictionary<string, ComponentLoaderDelegate> _loaders;

    public EntityFactory(World world, List<IComponentLoader> loaders)
    {
        _world = world;
        _templates = [];
        _loaders = [];
        MapComponentsToLoaders();
        loaders.ForEach(l => AddCustomLoader(l));
    }

    private static string ClassToComponentKey(string raw, string suffix)
        => raw[..raw.LastIndexOf(suffix)].FirstCharToLower();

    /// <summary>
    /// Adds a custom loader to the loader dictionary.
    /// </summary>
    /// <param name="loader"></param>
    public void AddCustomLoader(IComponentLoader loader)
    {
        _loaders[ClassToComponentKey(loader.GetType().Name, ComponentIdentifier)]
            = (e, data) => loader.LoadComponent(e, data);
    }

    public static ComponentLoaderDelegate CreateTypedLoader<T>()
    {
        return (e, data) =>
        {
            var component = DataLoader.DeserObject<T>(data.GetRawText());
            e.Set(component);
        };
    }

    /// <summary>
    /// From the classes/structs in our assembly that match our component identifier,
    /// add them to the loader dictionary with a default loading method.
    /// This should only be run once during initialization.
    /// </summary>
    /// <remarks>
    /// This should eventually be refactored because it relies on hacky string manipulation
    /// </remarks>
    public void MapComponentsToLoaders()
    {
        // get all classes/structs that match our component naming scheme
        var components = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.Name.EndsWith(ComponentIdentifier)
                && (t.IsClass || t.IsValueType))
            .ToList();

        foreach (var componentType in components)
        {
            var loaderKey = ClassToComponentKey(componentType.Name, ComponentIdentifier);
            // create a loader for handling loading of that specific component type
            var typedLoader = typeof(EntityFactory)
                .GetMethod(nameof(CreateTypedLoader), BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(componentType)
                .Invoke(null, null) as ComponentLoaderDelegate;

            // add the new typed loader to the loader dictionary
            _loaders[loaderKey] = typedLoader;
        }
    }

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
        if (id != EntityID.ControlMarker)
            Console.WriteLine($"loading {id}");
        var entity = _world.CreateEntity();
        if (!_templates.TryGetValue(id, out EntityTemplateDef template))
            throw new KeyNotFoundException($"[ERROR] {id} was not found in the entity templates.");

        foreach (var componentRep in template.Components)
        {
            var cmpName = componentRep.Key;
            var cmpData = componentRep.Value;
            if (!_loaders.TryGetValue(cmpName, out ComponentLoaderDelegate loader))
            {
                Console.WriteLine($"No loader found for {cmpName}");
                continue;
            }
            loader(entity, cmpData);
        }
        if (id != EntityID.ControlMarker)
            Console.WriteLine($"loaded {id}");
        return entity;
    }

    public Entity CreateEntity(string id, Point pos)
        => CreateEntity(id, pos.X, pos.Y);

    public Entity CreateEntity(string id, int x, int y)
    {
        if (id != EntityID.ControlMarker)
            Console.WriteLine($"loading {id} (1)");
        var entity = CreateEntity(id);
        entity.Set<GridPosComponent>(new(x, y));
        return entity;
    }
}