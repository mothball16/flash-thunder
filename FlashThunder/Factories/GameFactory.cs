using DefaultEcs.System;
using FlashThunder.Core;
using FlashThunder.ECSGameLogic.Components;
using FlashThunder.ECSGameLogic.Resources;
using FlashThunder.ECSGameLogic.Systems.OnDraw;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Bridges;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Debugging;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Input;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Reactive;
using FlashThunder.Enums;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Core;
using FlashThunder.ECSGameLogic.Systems.OnUpdate.Units;
using FlashThunder.States;
using Microsoft.Xna.Framework;
using FlashThunder._ECSGameLogic.Misc;
using FlashThunder.Utilities;

namespace FlashThunder.Factories
{
    /// <summary>
    /// Holds the bigass InitContext method so that GameRunningState is not taken up 50% by creation
    /// stuff
    /// </summary>
    internal class GameFactory : IGameStateFactory
    {
        // lasting dependencies in-between sessions
        private readonly EventBus _eventBus;
        private readonly TexManager _texManager;
        private readonly InputManager<GameAction> _gameInputManager;
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
        private GameContext InitContext()
        {
            // set up the camera
            var camera = new Camera();

            // set up the ecs world
            var world = new World();

            // TODO: REMOVE THIS ONCE A PROPER INITIALIZATION IS READY
            InitWorld(world);

            // set up the entity factory
            var factory = new EntityFactory(world, _texManager)
                .LoadTemplates("entity_templates.json")
                .LoadTemplates("unit_templates.json");

            // set up the connections between higher systems and the ecs architecture
            // input is for all input event transmission
            // mouse is for frame-by-frame updates of specifically the mouse
            var inputBridge = new InputMediator(world, _gameInputManager);
            var mousePolling = new MousePollingSystem(world, _gameInputManager, camera);
            var cameraControl = new CameraControlSystem(world, camera);
            var actionUpdate = new ActionPollingSystem(world);

            // initialize the systems (update)
            var entityCounting = new EntityCountingSystem(_eventBus, world);
            var playerCommand = new PlayerCommandSystem(world);
            var playerCameraInput = new PlayerCameraInputSystem(world);

            var playerDebuggingInput = new PlayerDebuggingInputSystem(world);
            var debuggingGeneral = new DebugSystem(world);

            var controlledEntityMarker = new ControlledEntityMarkerSystem(world);

            var spawnerSystem = new SpawnProcessingSystem(world, factory);
            var entityDisposalTicker = new EntityDisposalTickerSystem(world);
            var entityFinalDisposal = new EntityFinalDisposalSystem(world);

            var _updateSystems = new SequentialSystem<float>(
                actionUpdate, // Updates active actions
                mousePolling, // Updates mouse resource
                entityCounting, // (DEBUGGING) Testing entity count

                

                playerCommand, // Processes any player game commands on this frame
                playerCameraInput, // Processes any camera-specific commands on this frame

                playerDebuggingInput, // (DEBUGGING) Testing input response
                debuggingGeneral, // (DEBUGGING) General debugging system

                controlledEntityMarker, // Places a marker on all controlled entities

                spawnerSystem, // Handles any RequestSpawns
                entityDisposalTicker, // Ticks the entity disposal system
                entityFinalDisposal, // Actually removes the entities
                cameraControl // Updates the physical camera in preparation for render
                );

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            // initialize the systems (draw)
            var sbInit = new SpriteBatchInitSystem(world);
            var tileMapRender = new TileMapRenderSystem(world, _tileManager);
            var entityRender = new EntityRenderSystem(world);
            var selectedTileRender = new SelectedTileRenderSystem(world);

            var _drawSystems = new SequentialSystem<SpriteBatch>(
                sbInit, // Using our current environment, begin SB based off it
                tileMapRender, // Tilemap to be rendered at the bottom
                entityRender, // Entities to be rendered on top of tilemap
                selectedTileRender // Selected tile to be rendered on top of entities
                );

            // send back the initialized GameContext
            return new GameContext(
                world: world,
                factory: factory,
                assetManager: _texManager,
                inputBridge: inputBridge,
                onUpd: _updateSystems,
                onDraw: _drawSystems
                );
        }

        private void InitWorld(World world)
        {
            // FOR NOW: manually initialize the map
            var tileMap = new TileMapComponent()
            {
                Map = [
                    ['#', '.', '#', '#', '#'],
                    ['.', '#', '.', '#', '#']
                ],
            };

            var mapSettings = new EnvironmentResource()
            {
                TileSize = 64,
                FocusedTeam = "client",
                Teams = ["client", "enemy"]
            };

            // optimized thing for position lookups ( the old solution was checking every entity Lol )
            var entityPositions = world.GetEntities()
                .With<GridPosComponent>()
                .AsMultiMap(new DelegatedEqualityComparer<GridPosComponent>(
                    (obj) => HashCode.Combine(obj.X, obj.Y),
                    (x,y) => (x.X == y.X && x.Y == y.Y)
                    ));

            world.Set(tileMap);
            world.Set(mapSettings);
            world.Set(entityPositions);
        }


        public IGameState Create()
        {
            var context = InitContext();
            InitWorld(context.World);
            return new GameRunningState(_eventBus, context);
        }
    }
}
