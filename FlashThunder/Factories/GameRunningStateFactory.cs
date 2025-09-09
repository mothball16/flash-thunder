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
using FlashThunder.GameLogic;
using FlashThunder.GameLogic.Components;
using FlashThunder.GameLogic.Commands;
using FlashThunder.GameLogic.Events;
using Microsoft.Xna.Framework;
using System.Linq;
using FlashThunder.GameLogic.Movement.Components;
using FlashThunder.GameLogic.Input.Resources;
using FlashThunder.GameLogic.Movement.Systems;
using FlashThunder.GameLogic.Input.Systems;
using FlashThunder.GameLogic.Rendering.Systems;
using FlashThunder.GameLogic.Movement.Services;
using FlashThunder.GameLogic.Team.Components;
using FlashThunder.GameLogic.CameraControl.Components;
using FlashThunder.GameLogic.Cleanup.Systems;
using FlashThunder.GameLogic.CameraControl.Systems;
using FlashThunder.GameLogic.CameraControl.Handlers;
using FlashThunder.GameLogic.Selection.Components;
using FlashThunder.GameLogic.Rendering.Components;
using FlashThunder.GameLogic.Team.Services;
using FlashThunder.GameLogic.Selection.Systems;
using FlashThunder.Screens.Handlers;
using FlashThunder.GameLogic.Actions;
using FlashThunder.GameLogic.Actions.Behaviors;
using FlashThunder.GameLogic.Actions.Systems;
using FlashThunder.GameLogic.Actions.Data;
using FlashThunder.GameLogic.Actions.Components;
using FlashThunder.GameLogic._Shared.ComponentLoaders;
using FlashThunder.GameLogic._Shared.Services;

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
    private readonly ScreenManager _screenManager;
    //TODO: when JSON map loading is up, make the tile manager session-specific
    private readonly TileManager _tileManager;

    public GameRunningStateFactory(
        EventBus eventBus,
        InputManager<GameAction> gameInputManager,
        TextureManager texManager,
        ScreenManager screenManager,
        TileManager tileManager)
    {
        _gameInputManager = gameInputManager;
        _texManager = texManager;
        _screenManager = screenManager;
        _tileManager = tileManager;
        _eventBus = eventBus;
    }

    private void InitResources(World world)
    {
        // FOR NOW: provide an empty mouse resource as placeholder
        var mouseResource = new MouseResource();
        var turnOrderResource = new TurnOrderResource();
        // this provides a read-only version of the input manager
        var inputResource = new InputResource
        {
            Input = _gameInputManager,
            ConsumedInputs = []
        };
        // FOR NOW: manually initialize the map, we will do actual loading later.
        var mapResource = new MapResource
        {
            Tiles = [
                ['.', '#', '.', '#', '.', '#'],
                ['.', '#', '.', '#', '.', '#'],
                ['.', '#', '#', '#', '.', '.'],
                ['.', '.', '.', '#', '#', '#'],
                ['.', '#', '.', '.', '#', '.'],
                ['.', '#', '#', '#', '#', '#']
            ],
        };
        var uiNotifyResource = _eventBus as IEventPublisher;

        world.Set(mouseResource);
        world.Set(turnOrderResource);
        world.Set(inputResource);
        world.Set(mapResource);
        world.Set(uiNotifyResource);
    }

    private void InitServices(World world, EntityFactory factory)
    {
        var lookupService = new LookupService(world);
        var teamService = new TeamService(world, factory);
        var mapResource = world.Get<MapResource>();
        #region - - - [pathfinding service ] - - -
        var pathfindingService = new PathfindingService(
            map: mapResource,
            isPassable: (Point pos, string[] canTraverse, int size) =>
            {
                var tileDef = _tileManager
                .GetTileDefinition(mapResource.Tiles[pos.Y][pos.X]);

                foreach (var traverse in canTraverse)
                {
                    if (tileDef.Traverse.Contains(traverse))
                        return true;
                }
                return false;
            },
            pathfindingHeuristic: (Point a, Point b) =>
            {
                return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            },
            getNodeWeight: (Point pos) =>
            {
                var tile = mapResource.Tiles[pos.Y][pos.X];
                return _tileManager.GetTileDefinition(tile).Cost;
            });
        #endregion

        world.Set(lookupService);
        world.Set(teamService);
        world.Set(pathfindingService);
    }

    /// <summary>
    /// Builds the teams for now. When JSON loading is ready this will be replaced for
    /// a data-oriented loader.
    /// </summary>
    /// <param name="world"></param>
    public static void InitTeams(World world)
    {
        var teamService = world.Get<TeamService>();
        ref var turnOrder = ref world.Get<TurnOrderResource>();
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
        List<AUpdateSystem<float>> updateSystems = [];
        List<AUpdateSystem<SpriteBatch>> drawSystems = [];
        List<AUpdateSystem<float>> postCycleSystems = [];
        List<IDisposable> disposables = [];

        // initialize the physical camera
        var camera = new Camera();

        // initialize the ECS world
        var world = new World()
            .InitializeExtensions();

        // set up the entity factory
        var factory = new EntityFactory(world)
            .LoadTemplates("internal_templates.json")
            .LoadTemplates("entity_templates.json")
            .LoadTemplates("unit_templates.json")
            .Map<Health>(new HealthComponentLoader())
            .Map<SpriteData>(new SpriteDataComponentLoader(_texManager))
            .Map<GridMover>(new GridMoverLoader())
            .Map<SkillSet>(new SkillSetLoader())
            .Map<Vision>()
            .Map<GridPosition>().Map<WorldPosition>()
            .Map<Armor>()
            .Map<SelectedTag>().Map<SelectableTag>()
            .Map<ActiveCamera>().Map<WorldCamera>()
            .Map<SmoothScalable>()
            .Map<IsPlayerControllable>();

        var attackManager = new ActionLifetimeManager()
            .RegisterActionBehavior(new MoveToBehavior())
            .RegisterActionBehavior(new BasicAttackBehavior());

        // set up the environment
        InitResources(world);
        InitServices(world, factory);
        InitTeams(world);

        // - - - [ system initialization ] - - -

        // [!] update
        // core
        var pollForMouseData = new MousePollingSystem(world, camera);
        var moveEntities = new EntityMoverSystems(world);

        // game logic
        var calculateTilesOfActions = new ActionTileCalcSystem(world);
        var selectUnitsOnClick = new UnitSelectionSystem(world);
        var selectAbilitiesOnKey = new AbilitySelectSystems(world);

        var queueActions = new QueueActionSystem(world);

        var executeQueuedActions = new ActionExecutionSystem(world, attackManager);
        var processTakeDamageInflict = new TakeDamageProcessingSystem(world);

        // pre-render (post-update)
        var interpWorldToGridMovers = new GridMoverSystem(world);
        var updateCameras = new CameraSystems(world, camera);

        // [!] rendering
        var initiateSpriteBatch = new RenderInitSystem(camera);
        var renderTiles = new TileRenderSystem(world, _tileManager);
        var renderEntities = new EntityRenderSystems(world);
        var renderDecorators = new DecoratorSystems(world, _texManager);

        // [!] post-cycle
        var janitor = new JanitorSystems(world);

        updateSystems.AddRange([
            pollForMouseData,
            moveEntities,

            calculateTilesOfActions,
            selectUnitsOnClick,
            selectAbilitiesOnKey,

            queueActions,

            // we've already finished selection and checks to make sure action is valid
            executeQueuedActions,
            processTakeDamageInflict,

            interpWorldToGridMovers,
            updateCameras,
        ]);

        drawSystems.AddRange([
            initiateSpriteBatch,
            renderTiles,
            renderDecorators,
            renderEntities,
        ]);

        postCycleSystems.AddRange([
            janitor
        ]);

        // - - - [ event handler initialization ] - - -
        var nextTurn = new NextTurnHandler(world);
        var spawnPrefab = new SpawnPrefabHandler(world, factory);
        var camChange = new CameraChangeHandler(world);
        disposables.AddRange([
            nextTurn,
            spawnPrefab,
            camChange
            ]);

        // - - - [ final world setup ] - - -
        world.Publish<SpawnPrefabRequest>(new("internal_init_camera"));
        for(int i = 0; i < 2; i++)
        {
            world.Publish<SpawnPrefabRequest>(new()
            {
                Name = "infantry_scout",
                Position = new(1 + i, 1),
                Team = "Section 4"
                /*
                Callback = (Entity e) =>
                {
                    e.Add(new SkillSet()
                    {
                        Skills = [
                            new SkillEntry(){
                                Data = new() {
                                    Name = "Movement",
                                    Icon = "unit_action_move_unit_frame",
                                    Description = "Move to an accessible tile within range.",
                                    CooldownBetweenTurns = 0,
                                    UsesPerTurn = 1,
                                    Range = 3,
                                    Traverse = ["land"],
                                    SelectionType = SelectionType.Pathfinding,
                                    AttackBehavior = "MoveToBehavior",
                                    AttackParams = new EmptyParams()
                                },
                                State = new() { TurnsSinceLastUse = 0, UsesLeftThisTurn = 1,  CanUse=true}
                            },
                            new SkillEntry(){
                                Data = new()
                                {
                                    Name = "Hit and Run",
                                    Icon = "unit_action_attack_placeholder_frame",
                                    Description = "Mildly inconvenience your enemies with this one simple trick!",
                                    CooldownBetweenTurns = 0,
                                    UsesPerTurn = 2,
                                    Range = 2,
                                    Traverse = ["land"],
                                    SelectionType = SelectionType.Passthrough,
                                    AttackBehavior = "BasicAttackBehavior",
                                    AttackParams = new DefaultAttackParams(10, 2, 0)
                                },
                                State = new() { TurnsSinceLastUse = 0, UsesLeftThisTurn = 2, CanUse = true}
                            },
                        ]
                    });
                }*/
            });
        }
        
        return new GameRunningState(world, _screenManager, updateSystems, drawSystems, postCycleSystems, disposables);
    }
}