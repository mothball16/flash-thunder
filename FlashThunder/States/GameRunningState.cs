using Dcrew.MonoGame._2D_Camera;
using DefaultEcs;
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
using FlashThunder.Events;
using FlashThunder.Interfaces;
using FlashThunder.Managers;
using FlashThunder.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace FlashThunder.States
{
    public class GameRunningState : IGameState
    {
        private GameContext _context;
        private readonly TexManager _texManager;
        private readonly InputManager<GameAction> _gameInputManager;
        private readonly TileManager _tileManager;
        public GameRunningState(
            TexManager texManager,
            InputManager<GameAction> gameInputManager,
            TileManager tileManager)
        {
            _gameInputManager = gameInputManager;
            _texManager = texManager;
            _tileManager = tileManager;
        }

        private GameContext InitContext()
        {
            // set up the camera
            var camera = new Camera();

            // set up the ecs world
            var world = new World();

            // set up the entity factory
            var factory = new EntityFactory(world, _texManager)
                .LoadTemplates("entity_templates.json")
                .LoadTemplates("unit_templates.json");

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
            };

            world.Set(tileMap);
            world.Set(mapSettings);

            // set up the connections between higher systems and the ecs architecture
            // input is for all input event transmission
            // mouse is for frame-by-frame updates of specifically the mouse
            var inputBridge = new InputMediator(world, _gameInputManager);
            var mousePollingSystem = new MousePollingSystem(world, _gameInputManager, camera);
            var cameraControlSystem = new CameraControlSystem(world, camera);
            var actionUpdateSystem = new ActionPollingSystem(world);

            // initialize the systems (update)
            var entityCountingSystem = new DebugSystem(world);
            var playerCommandSystem = new PlayerCommandSystem(world);
            var playerCameraInputSystem = new PlayerCameraInputSystem(world);
            var playerDebuggingInputSystem = new PlayerDebuggingInputSystem(world);

            var spawnerSystem = new SpawnProcessingSystem(world, factory);

            var _updateSystems = new SequentialSystem<float>(
                actionUpdateSystem, // Updates active actions
                mousePollingSystem, // Updates mouse resource
                entityCountingSystem, // (DEBUGGING) Testing entity count

                playerCommandSystem, // Processes any player game commands on this frame
                playerCameraInputSystem, // Processes any camera-specific commands on this frame
                playerDebuggingInputSystem, // (DEBUGGING) Testing input response

                spawnerSystem, // Handles any RequestSpawns
                cameraControlSystem // Updates the physical camera in preparation for render
                );

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            // initialize the systems (draw)
            var sbInitSystem = new SpriteBatchInitSystem(world);
            var tileMapRenderSystem = new TileMapRenderSystem(world, _tileManager);
            var entityRenderSystem = new EntityRenderSystem(world);

            var _drawSystems = new SequentialSystem<SpriteBatch>(
                sbInitSystem, // Using our current environment, begin SB based off it
                tileMapRenderSystem, // Tilemap to be rendered at the bottom
                entityRenderSystem // Entities to be rendered on top of tilemap
                );

            // send back the initialized GameContext
            return new GameContext(
                world: world,
                assetManager: _texManager,
                inputBridge: inputBridge,
                onUpd: _updateSystems,
                onDraw: _drawSystems
                );
        }

        public void Enter()
        {
            _context = InitContext();
            EventBus.Publish<LoadScreenEvent>(new()
            {
                ScreenFactory = () => new GameScreen().Visual,
                Layer = ScreenLayer.Primary
            });
        }
        public void Exit()
        {
            _context?.Dispose();
        }
        public void Update(float dt)
        {
            _context.Update(dt);
        }
        public void Draw(SpriteBatch sb)
        {

            _context.Draw(sb);
        }
    }
}
