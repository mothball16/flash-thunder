using DefaultEcs.System;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Systems.OnDraw;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Bridges;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Debugging;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;
using FlashThunder.Enums;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using DefaultEcs;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Units;
using FlashThunder.States;
using FlashThunder.Utilities;
using FlashThunder.ECSGameLogic.ComponentLoaders;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Set.Reactive;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Core;
using FlashThunder._ECSGameLogic;
using FlashThunder.Defs;
using FlashThunder._ECSGameLogic.Components.TeamStats;

namespace FlashThunder.Factories;

/// <summary>
/// Holds the bigass InitContext method so that GameRunningState is not taken up 50% by creation
/// stuff
/// </summary>
internal class GameFactory : IGameStateFactory
{
    // lasting dependencies in-between sessions
    private readonly EventBus _eventBus;
    private readonly InputManager<GameAction> _gameInputManager;
    private readonly TexManager _texManager;
    private readonly TileManager _tileManager;

    public GameFactory(
        EventBus eventBus,
        InputManager<GameAction> gameInputManager,
        TexManager texManager,
        TileManager tileManager)
    {
        _gameInputManager = gameInputManager;
        _texManager = texManager;
        _tileManager = tileManager;
        _eventBus = eventBus;
    }



    //TODO: REPLACE THE BIG SYSTEM INITIALIZATION BLOCK WITH HELPERS THAT INITIALIZE RELATED SYSTEMS
    private GameContext InitContext()
    {
        // set up the camera
        var camera = new Camera();

        // set up the ecs world
        var world = new World();

        // TODO: REMOVE THIS ONCE A PROPER INITIALIZATION IS READY
        InitWorld(world);
        InitTurnOrder(world);

        List<IComponentLoader> componentLoaders = [
            new HealthComponentLoader(),
            new SpriteDataComponentLoader(_texManager)
            ];

        // set up the entity factory
        var factory = new EntityFactory(world, componentLoaders)
            .LoadTemplates("entity_templates.json")
            .LoadTemplates("unit_templates.json");
        
        var cameraControl = new CameraControlSystem(world, camera);

        // initialize the systems (update)
        var entityCounting = new EntityCountingSystem(_eventBus, world);

        var _updateSystems = new SequentialSystem<GameFrameSnapshot>(
            entityCounting, // (DEBUGGING) Testing entity count
            CreateEntityManagementSystems(world),
            CreatePlayerControlSystems(world),
            CreateRequestHandlerSystems(world, factory),
            CreateDisposalSystems(world),

            cameraControl // Updates the physical camera in preparation for render
            );

        // initialize the systems (draw)
        var _drawSystems = new SequentialSystem<DrawFrameSnapshot>(
                CreateRenderSystems(world)
            );

        // send back the initialized GameContext
        return new GameContext {
            AssetManager = _texManager,
            InputManager = _gameInputManager,
            World = world,
            Factory = factory,
            UpdateSystems = _updateSystems,
            DrawSystems = _drawSystems,
            ActionSnapshotCreator = new ActionSnapshotCreator(world),
            Camera = camera,
        };
    }

    private static SequentialSystem<GameFrameSnapshot> CreateDisposalSystems(World world)
    {
        var entityDisposalInFramesTicker = new EntityDisposalInFramesTickerSystem(world);
        var entityDisposalInSecondsTicker = new EntityDisposalInSecondsTickerSystem(world);
        var entityFinalDisposal = new EntityFinalDisposalSystem(world);
        return new(
            entityDisposalInFramesTicker,
            entityDisposalInSecondsTicker,
            entityFinalDisposal
            );
    }

    private static SequentialSystem<GameFrameSnapshot> CreatePlayerControlSystems(World world)
    {
        var playerCommand = new PlayerCommandSystem(world);
        var playerCameraInput = new PlayerCameraInputSystem(world);
        var playerDebuggingInput = new PlayerDebuggingInputSystem(world);
        return new(
            playerCommand, // Processes any player game commands on this frame
            playerCameraInput, // Processes any camera-specific commands on this frame
            playerDebuggingInput // (DEBUGGING) Testing input response
            );
    }

    private SequentialSystem<GameFrameSnapshot> CreateRequestHandlerSystems(World world, EntityFactory factory)
    {
        var spawner = new SpawnProcessingSystem(world, factory);
        var unitSelection = new UnitSelectionSystem(world);
        var turnOrder = new TurnProcessingSystem(world, _eventBus);
        return new(
            spawner, // Processes any spawn requests
            unitSelection, // Processes any unit selection requests
            turnOrder // Processes the turn order by cycling to the next team on request
            );
    }
    private static SequentialSystem<GameFrameSnapshot> CreateEntityManagementSystems(World world)
    {
        var controlledEntityMarker = new ControlledEntityMarkerSystem(world);
        return new(
            controlledEntityMarker // Places a marker on all controlled entities
            );
    }

    private SequentialSystem<DrawFrameSnapshot> CreateRenderSystems(World world)
    {
        var sbInit = new SpriteBatchInitSystem(world);
        var tileMapRender = new TileMapRenderSystem(world, _tileManager);
        var entityRender = new EntityRenderSystem(world);
        var selectedTileRender = new SelectedTileRenderSystem(world);

        return new(
            sbInit, // Using our current environment, begin SB based off it
            tileMapRender, // Tilemap to be rendered at the bottom
            entityRender, // Entities to be rendered on top of tilemap
            selectedTileRender // Selected tile to be rendered on top of entities
            );
    }


    private void InitWorld(World world)
    {
        // FOR NOW: manually initialize the map, we will do actual loading later.
        var tileMap = new TileMapComponent()
        {
            Map = [
                ['#', '.', '#', '#', '#'],
                ['.', '#', '.', '#', '#']
            ],
        };

        // optimized thing for position lookups ( the old solution was checking every entity Lol )
        var entityPositions = world.GetEntities()
            .With<GridPosComponent>()
            .AsMultiMap(new DelegatedEqualityComparer<GridPosComponent>(
                (obj) => HashCode.Combine(obj.X, obj.Y),
                (x, y) => (x.X == y.X && x.Y == y.Y)
                ));

        world.Set(tileMap);
        world.Set(entityPositions);
    }

    private static void InitTurnOrder(World world)
    {
        // FOR NOW: manually initialize the teams and turn orders, we will do actual loading later.
        var turnOrder = new TurnOrderResource();
        var myTeam = world.CreateEntity();
        var notMyTeam = world.CreateEntity();
        myTeam.Set(new TeamTagComponent("player"));
        myTeam.Set(new IsCurrentTurnComponent());
        myTeam.Set(new EnemiesWithComponent([notMyTeam]));

        notMyTeam.Set(new TeamTagComponent("enemy"));
        notMyTeam.Set(new EnemiesWithComponent([myTeam]));

        turnOrder.Order.Enqueue(myTeam);
        turnOrder.Order.Enqueue(notMyTeam);

        world.Set(turnOrder);
    }

    public IGameState Create()
    {
        var context = InitContext();
        InitWorld(context.World);
        return new GameRunningState(_eventBus, context);
    }
}