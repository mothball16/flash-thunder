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
using FlashThunder.ECSGameLogic.Components.UnitStats;
using FlashThunder.GameLogic.Resources;
using FlashThunder.Extensions;
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder._ECSGameLogic.Components.TeamStats;
using FlashThunder.GameLogic.Commands;
using FlashThunder.GameLogic.Systems.OnUpdate;
using FlashThunder.GameLogic.Systems.OnDraw;

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

    /// <summary>
    /// Builds the teams for now. When JSON loading is ready this will be replaced for
    /// a data-oriented loader.
    /// </summary>
    /// <param name="world"></param>
    public void InitTeams(World world, EntityFactory factory)
    {
        ref var turnOrder = ref world.GetResource<TurnOrderResource>();

        var ourTeam = factory.CreateTeamBundle(
            new TeamTag("Section 4"),
            new Faction("Northern Alliance"),
            new IsPlayerControllable());

        var enemyTeam = factory.CreateTeamBundle(
            new TeamTag("Southern Special Warfare Command"),
            new Faction("Southern Coalition"));
        turnOrder.Order.Add(ourTeam);
        turnOrder.Order.Add(enemyTeam);
    }

    public IGameState Create()
    {
        List<IUpdateSystem<float>> updateSystems = [];
        List<IUpdateSystem<SpriteBatch>> drawSystems = [];

        // initialize the physical camera
        var camera = new Camera();

        // initialize the ECS world
        var world = new World();

        // set up the entity factory
        var factory = new EntityFactory(world)
            .LoadTemplates("entity_templates.json")
            .LoadTemplates("unit_templates.json")
            .LoadTemplates("internal_templates.json")
            .Map<Health>(new HealthComponentLoader())
            .Map<SpriteData>(new SpriteDataComponentLoader(_texManager))
            .Map<Move>().Map<MoveIntent>()
            .Map<Vision>().Map<MaxRange>()
            .Map<GridPosition>().Map<WorldPosition>()
            .Map<Armor>()
            .Map<SelectedTag>()
            .Map<SpriteData>()
            .Map<ActiveCamera>().Map<WorldCamera>();

        // set up the environment
        InitResources(world);
        InitTeams(world, factory);


        // - - - [ system initialization ] - - -

        // camera control
        var cameraSystems = new CameraSystems(world, camera);

        // rendering
        var sbInit = new RenderInitSystem(camera);
        var tileRender = new TileRenderSystem(world, _tileManager);
        var entityRender = new EntityRenderSystem(world);

        updateSystems.AddRange([
            cameraSystems
        ]);

        drawSystems.AddRange([
            sbInit,
            tileRender,
            entityRender
        ]);

        // - - - [ commands to complete world initialization ] - - -
        // set up default active camera
        factory.CreatePrefab("internal_init_camera");

        // call the command to run
        new NextTurnCommand(world, _eventBus).Execute();

        return new GameRunningState(_eventBus, updateSystems, drawSystems);
    }
}