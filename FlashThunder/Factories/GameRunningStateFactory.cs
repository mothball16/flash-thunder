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
        world.SetResource(mouseResource);
        world.SetResource(turnOrderResource);
        world.SetResource(inputResource);
        world.SetResource(mapResource);
    }

    private void InitServices(World world, EntityFactory factory)
    {
        var teamService = new TeamService(world, factory);
        var mapResource = world.GetResource<MapResource>();
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

        world.SetResource(teamService);
        world.SetResource(pathfindingService);
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
            .Map<Vision>()
            .Map<GridPosition>().Map<WorldPosition>()
            .Map<Armor>()
            .Map<SelectedTag>().Map<SelectableTag>()
            .Map<ActiveCamera>().Map<WorldCamera>()
            .Map<SmoothScalable>()
            .Map<IsPlayerControllable>();

        var attackManager = new ActionManager()
            .RegisterAttackBehavior(new MoveToBehavior())
            .RegisterAttackBehavior(new BasicAttackBehavior());

        // set up the environment
        InitResources(world);
        InitServices(world, factory);
        InitTeams(world);

        // - - - [ system initialization ] - - -

        // [!] update
        // core
        var mousePolling = new MousePollingSystem(world, camera);
        var entityMover = new EntityMoverSystems(world);

        // game logic
        var actionCalc = new ActionTileCalcSystem(world);
        var unitSelection = new UnitSelectionSystem(world);
        var abilitySelection = new SelectedUnitAbilitySelectionSystem(world, _eventBus);

        var unitMove = new PlayerActionTriggerSystem(world);

        var attackExecution = new ActionExecutionSystem(world, attackManager);
        var takeDamageProcessing = new TakeDamageProcessingSystem(world);

        // pre-render (post-update)
        var worldMoveToGridPos = new GridMoverSystem(world);
        var cameraSystems = new CameraSystems(world, camera);

        // [!] rendering
        var sbInit = new RenderInitSystem(camera);
        var tileRender = new TileRenderSystem(world, _tileManager);
        var entityRender = new EntityRenderSystems(world);
        var decorators = new DecoratorSystems(world, _texManager);

        // [!] post-cycle
        var janitor = new JanitorSystems(world);

        updateSystems.AddRange([
            mousePolling,
            entityMover,

            actionCalc,
            unitSelection,
            abilitySelection,

            unitMove,

            attackExecution,
            takeDamageProcessing,

            worldMoveToGridPos,
            cameraSystems,
        ]);

        drawSystems.AddRange([
            sbInit,
            tileRender,
            decorators,
            entityRender,
        ]);

        postCycleSystems.AddRange([
            janitor
        ]);

        // - - - [ event handler initialization ] - - -
        var nextTurn = new NextTurnHandler(world, _eventBus);
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
                Team = "Section 4",
                Callback = (Entity e) =>
                {
                    e.Add(new SkillSet()
                    {
                        Skills = [
                            new UnitSkill
                            {
                                Name = "Movement",
                                Icon = "unit_action_move_unit_frame",
                                Description = "Move to an accessible tile within range.",
                                Range = 3,
                                Traverse = ["land"],
                                Cooldown = 0,
                                SelectionType = SelectionType.Pathfinding,
                                AttackBehavior = "MoveToBehavior",
                                AttackParams = new EmptyParams()
                            },
                            new UnitSkill
                            {
                                Name = "Hit and Run",
                                Icon = "unit_action_attack_placeholder_frame",
                                Description = "Mildly inconvenience your enemies with this one simple trick!",
                                Range = 2,
                                Traverse = ["land"],
                                SelectionType = SelectionType.Passthrough,
                                AttackBehavior = "BasicAttackBehavior",
                                AttackParams = new DefaultAttackParams(10, 2, 0),
                            }
                        ]
                    });
                }
            });
        }
            
        return new GameRunningState(world, _screenManager, updateSystems, drawSystems, postCycleSystems, disposables);
    }
}