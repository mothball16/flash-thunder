using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FontStashSharp;
using TextCopy;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.Enums;
using DefaultEcs;
using DefaultEcs.System;
using System.Runtime.CompilerServices;
using FlashThunder.Managers;
using FlashThunder.Gameplay.Systems;
using FlashThunder.Gameplay.Components;
using FlashThunder.Gameplay.Resources;
using FlashThunder.Gameplay.Systems.OnUpdate.Bridges;
using FlashThunder.Gameplay.Systems.OnUpdate.Debugging;
using FlashThunder.Gameplay.Systems.OnUpdate.Input;
using FlashThunder.Gameplay.Systems.OnDraw;

namespace FlashThunder.Core
{
    public class CoreGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameContext _context;
        private GameState _gameState;
        //TODO: This should just be a menuAction input manager.
        //The game input manager should only exist within the game runtime
        private InputManager<PlayerAction> _inputManager;

        //TODO: Same for this. We should still ahve an assetmanager for the menu tho.
        //Actually think about this abit because we don't want to reload textures all the time
        private TexManager _assetManager;
        private TileManager _tileManager;

        public CoreGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // - - - [ Initialize higher systems ] - - -
            
            _inputManager = new InputManager<PlayerAction>()
                .BindAction(Keys.W, PlayerAction.Jump)
                .BindAction(Keys.S, PlayerAction.Crouch)
                .BindAction(Keys.D, PlayerAction.MoveRight)
                .BindAction(Keys.A, PlayerAction.MoveLeft);
            _assetManager = new TexManager(Content, "clearTile");
            _tileManager = new TileManager();

            _context = InitGameContext();

            _gameState = GameState.Running;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //set default (for now)

            _assetManager
                .LoadDefinitions("texture_manifest.json");

            _tileManager
                .LoadDefinitions(_assetManager, "tile_defs.json");
        }

        protected override void Update(GameTime gameTime)
        {
            // - - - [ Higher system updates ] - - -
            _inputManager.Update();
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // - - - [ ECS updates ] - - -
            _context.Update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (_gameState)
            {
                case GameState.Menu:
                    _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                        SamplerState.PointClamp, null, null, null);
                    break;
                case GameState.Running:
                    //The spritebatch is begun within the ECS architecture.
                    _context.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        #region - - - [ Helpers ] - - - 
        /// <summary>
        /// Helper method for creating the game context.
        /// EVentually to be moved to a GameStateManager
        /// </summary>
        /// <returns>The initialized GameContext.</returns>
        private GameContext InitGameContext()
        {

            //FOR NOW: manually initialize the map
            var tileMap = new TileMapComponent()
            {
                Map = [
                    ['#','.','#','#','#'],
                    ['.','#','.','#','#']
                ],
            };
            var mapSettings = new EnvironmentResource()
            {
                TileSize = 64
            };

            var camResource = new CameraResource()
            {
                Target = new Vector2(0,0),
                Offset = new Vector2(0,0),
                TweenFactor = 0.5f,
            };
            
            //set up the camera
            var camera = new Camera();


            //set up the ecs world
            var world = new World();
            world.Set(tileMap);
            world.Set(mapSettings);
            world.Set(camResource);
            //set up the connections between higher systems and the ecs architecture
            //input is for all input event transmission
            //mouse is for frame-by-frame updates of specifically the mouse
            var inputBridge = new InputBridge(world, _inputManager);
            var mousePollingSystem = new MousePollingSystem(world, _inputManager);
            var cameraControlSystem = new CameraControlSystem(world, camera);

            //initialize the systems (update)
            var entityCountingSystem = new DebugSystem(world);
            var commandSystem = new CommandSystem(world);
            var playerCameraInputSystem = new PlayerCameraInputSystem(world);

            var _updateSystems = new SequentialSystem<float>([
                mousePollingSystem,
                entityCountingSystem,

                commandSystem,
                playerCameraInputSystem,

                cameraControlSystem
                ]);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            //initialize the systems (draw)
            var sbInitSystem = new SpriteBatchInitSystem(world);
            var entityRenderSystem = new EntityRenderSystem(world); 
            var tileMapRenderSystem = new TileMapRenderSystem(world, _tileManager);

            var _drawSystems = new SequentialSystem<SpriteBatch>([
                sbInitSystem,
                entityRenderSystem,
                tileMapRenderSystem,
                ]);

            //send back the initialized GameContext
            return new GameContext(
                assetManager: _assetManager,
                world: world,
                inputBridge: inputBridge,
                onUpd: _updateSystems,
                onDraw: _drawSystems
                );
        
        }

        #endregion
    }
}