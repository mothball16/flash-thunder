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
        private InputManager<GameAction> _gameInputManager;

        //TODO: Same for this. We should still ahve an assetmanager for the menu tho.
        //Actually think about this abit because we don't want to reload textures all the time
        private TexManager _texManager;
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
            
            _gameInputManager = new InputManager<GameAction>()
                .BindAction(Keys.W, GameAction.MoveUp)
                .BindAction(Keys.S, GameAction.MoveDown)
                .BindAction(Keys.D, GameAction.MoveRight)
                .BindAction(Keys.A, GameAction.MoveLeft)
                .BindAction(Keys.LeftShift, GameAction.SpeedUpCamera)
                .BindAction(Keys.O, GameAction.SpawnTest);

            _texManager = new TexManager(Content, "clearTile");
            _tileManager = new TileManager();

            _context = InitGameContext();

            _gameState = GameState.Running;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //set default (for now)

            _texManager
                .LoadDefinitions("texture_manifest.json");

            _tileManager
                .LoadDefinitions(_texManager, "tile_defs.json");
        }

        protected override void Update(GameTime gameTime)
        {
            // - - - [ Higher system updates ] - - -
            _gameInputManager.Update();
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
        /// Giant method for now to handle game initialization while we're still developing the
        /// architecture. Eventually to be moved to a separate manager.
        /// </summary>
        /// <returns>The initialized GameContext.</returns>
        private GameContext InitGameContext()
        {
            //set up the camera
            var camera = new Camera();

            //set up the ecs world
            var world = new World();

            //set up the entity factory
            var factory = new EntityFactory(world, _texManager)
                .LoadTemplates("entity_templates.json")
                .LoadTemplates("unit_templates.json");

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

            world.Set(tileMap);
            world.Set(mapSettings);

            //set up the connections between higher systems and the ecs architecture
            //input is for all input event transmission
            //mouse is for frame-by-frame updates of specifically the mouse
            var inputBridge =               new InputBridge(world, _gameInputManager);
            var mousePollingSystem =        new MousePollingSystem(world, _gameInputManager, camera);
            var cameraControlSystem =       new CameraControlSystem(world, camera);
            var actionUpdateSystem =        new ActionPollingSystem(world);

            //initialize the systems (update)
            var entityCountingSystem =      new DebugSystem(world);
            var playerCommandSystem =       new PlayerCommandSystem(world);
            var playerCameraInputSystem =   new PlayerCameraInputSystem(world);
            var playerDebuggingInputSystem =new PlayerDebuggingInputSystem(world);

            var spawnerSystem =             new SpawnProcessingSystem(world, factory);

            var _updateSystems =            new SequentialSystem<float>([
                actionUpdateSystem, //Updates active actions
                mousePollingSystem, //Updates mouse resource
                entityCountingSystem, //(DEBUGGING) Testing entity count

                playerCommandSystem, //Processes any player game commands on this frame
                playerCameraInputSystem, //Processes any camera-specific commands on this frame
                playerDebuggingInputSystem, // (DEBUGGING) Testing input response

                spawnerSystem, //Handles any RequestSpawns
                cameraControlSystem //Updates the physical camera in preparation for render
                ]);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            //initialize the systems (draw)
            var sbInitSystem =              new SpriteBatchInitSystem(world);
            var tileMapRenderSystem =       new TileMapRenderSystem(world, _tileManager);
            var entityRenderSystem =        new EntityRenderSystem(world); 

            var _drawSystems =              new SequentialSystem<SpriteBatch>([
                sbInitSystem, //Using our current environment, begin SB based off it
                tileMapRenderSystem, // Tilemap to be rendered at the bottom
                entityRenderSystem, // Entities to be rendered on top of tilemap


                ]);

            //send back the initialized GameContext
            return new GameContext(
                assetManager: _texManager,
                world: world,
                inputBridge: inputBridge,
                onUpd: _updateSystems,
                onDraw: _drawSystems
                );
        
        }

        #endregion
    }
}