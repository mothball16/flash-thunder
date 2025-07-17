using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.Enums;
using FlashThunder.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.States;
using FlashThunder.ECSGameLogic.ComponentLoaders;
using fennecs;
using FlashThunder.GameLogic.Resources;
using FlashThunder.Extensions;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.GameLogic.Commands;
using FlashThunder.GameLogic.Systems.OnUpdate;
using FlashThunder.GameLogic.Systems.OnDraw;
using FlashThunder.GameLogic.Events;
using FlashThunder.GameLogic.Components.Relations;
using FlashThunder.GameLogic.Services;

namespace FlashThunder.Factories;

/// <summary>
/// Holds the bigass InitContext method so that GameRunningState is not taken up 50% by creation
/// stuff
/// </summary>
internal class GameRunningStateFactory : IGameStateFactory
{
    // lasting dependencies in-between sessions
    private readonly EventBus _eventBus;
    private readonly InputManager<GameAction> _gameInputManager;
    private readonly TextureManager _texManager;

    //TODO: when JSON map loading is up, make the tile manager session-specific
    private readonly TileManager _tileManager;

    public GameRunningStateFactory(
        EventBus eventBus,
        InputManager<GameAction> gameInputManager,
        TextureManager texManager,
        TileManager tileManager)
    {
        _gameInputManager = gameInputManager;
        _texManager = texManager;
        _tileManager = tileManager;
        _eventBus = eventBus;
    }

    private void InitResources(World world)
    {
        // FOR NOW: provide an empty mouse resource as placeholder
        var mouseResource = new MouseResource();
        var turnOrderResource = new TurnOrderResource();
        // this provides a read-only version of the input manager
        InputResource inputResource = _gameInputManager;
        // FOR NOW: manually initialize the map, we will do actual loading later.
        var mapResource = new MapResource
        {
            Tiles = [
                ['#', '.', '#', '#', '#'],
                ['.', '#', '.', '#', '#']
            ],
        };
        world.SetResource(mouseResource);
        world.SetResource(turnOrderResource);
        world.SetResource(inputResource);
        world.SetResource(mapResource);
    }

    private void InitServices(World world, EntityFactory factory)
    {
        var teamService = new TeamService(world, factory);
        world.SetResource(teamService);
    }

    /// <summary>
    /// Builds the teams for now. When JSON loading is ready this will be replaced for
    /// a data-oriented loader.
    /// </summary>
    /// <param name="world"></param>
    public void InitTeams(World world)
    {
        var teamService = world.GetResource<TeamService>();
        ref var turnOrder = ref world.GetResource<TurnOrderResource>();
        turnOrder.Order.Add(
            teamService.CreateTeam(
                "Section 4",
                "Northern Alliance",
                true));
        turnOrder.Order.Add(
            teamService.CreateTeam(
                "Southern Special Warfare Command",
                "Southern Coalition",
                false));
    }

    public IGameState Create()
    {
        List<IUpdateSystem<float>> updateSystems = [];
        List<IUpdateSystem<SpriteBatch>> drawSystems = [];
        List<IDisposable> disposables = [];

        // initialize the physical camera
        var camera = new Camera();

        // initialize the ECS world
        var world = new World().InitializeExtensions();

        // set up the entity factory
        var factory = new EntityFactory(world)
            .LoadTemplates("internal_templates.json")
            .LoadTemplates("entity_templates.json")
            .LoadTemplates("unit_templates.json")
            .Map<Health>(new HealthComponentLoader())
            .Map<SpriteData>(new SpriteDataComponentLoader(_texManager))
            .Map<Move>().Map<MoveIntent>()
            .Map<Vision>().Map<MaxRange>()
            .Map<GridPosition>().Map<WorldPosition>()
            .Map<Armor>()
            .Map<SelectedTag>().Map<SelectableTag>()
            .Map<ActiveCamera>().Map<WorldCamera>()
            .Map<Range>();

        // set up the environment
        InitResources(world);
        InitServices(world, factory);
        InitTeams(world);

        // - - - [ system initialization ] - - -

        var mousePolling = new MousePollingSystem(world, camera);
        var unitSelection = new UnitSelectionSystem(world);

        // camera control
        var cameraSystems = new CameraSystems(world, camera);

        // rendering
        var sbInit = new RenderInitSystem(camera);
        var tileRender = new TileRenderSystem(world, _tileManager);
        var entityRender = new EntityRenderSystem(world);
        var decorators = new DecoratorSystems(world, _texManager.Get("element_controlled_tile"));

        updateSystems.AddRange([
            mousePolling,
            unitSelection,
            cameraSystems
        ]);

        drawSystems.AddRange([
            sbInit,
            tileRender,
            entityRender,
            decorators
        ]);

        // - - - [ event handler initialization ] - - -
        var nextTurn = new NextTurnHandler(world, _eventBus);
        var spawnPrefab = new SpawnPrefabHandler(world, factory);

        disposables.AddRange([ 
            nextTurn,
            spawnPrefab
            ]);

        // - - - [ final world setup ] - - -
        
        world.Publish<SpawnPrefabRequestEvent>(new("internal_init_camera"));
        world.Publish<SpawnPrefabRequestEvent>(new()
        {
            Name = "infantry_scout",
            Position = new(0, 0),
            Team = "Section 4"
        });

        return new GameRunningState(world, _eventBus, updateSystems, drawSystems, disposables);
    }
}