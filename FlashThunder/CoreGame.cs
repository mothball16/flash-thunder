using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FontStashSharp;
using TextCopy;
using Dcrew.MonoGame._2D_Camera;
using FlashThunder.Enums;
using DefaultEcs;
using DefaultEcs.System;
using FlashThunder.Core.Systems;
using FlashThunder.Core.Components;
using System.Runtime.CompilerServices;
using FlashThunder.Managers;
using FlashThunder.ECS.Systems;
using FlashThunder.ECS.Resources;

namespace FlashThunder
{
    public class CoreGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameContext _context;

        //TODO: This should just be a menuAction input manager.
        //The game input manager should only exist within the game runtime
        private InputManager<PlayerAction> _inputManager;

        //TODO: Same for this. We should still ahve an assetmanager for the menu tho.
        //Actually think about this abit because we don't want to reload textures all the time
        private AssetManager _assetManager;

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
            _assetManager = new AssetManager();

            _context = InitGameContext();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //set default (for now)

            _assetManager
                .RegTex(Content.Load<Texture2D>("whiteSquare"))
                .RegTex('#', Content.Load<Texture2D>("tile_asteroid"))
                .RegTex('.', Content.Load<Texture2D>("clearTile"));
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
            _spriteBatch.Begin();

            // - - - [ Higher system renders] - - -

            // - - - [ ECS renders ] - - - 
            _context.Draw(_spriteBatch);


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
            var map = new TileMapComponent()
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

            //set up the ecs world
            var world = new World();
            world.Set<TileMapComponent>(map);
            world.Set<EnvironmentResource>(mapSettings);

            //set up the connections between higher systems and the ecs architecture
            //input is for all input event transmission
            //mouse is for frame-by-frame updates of specifically the mouse
            var inputBridge = new InputBridge(_inputManager, world);
            var mousePollingSystem = new MousePollingSystem(world, _inputManager);

            //initialize the systems (update)
            var entityCountingSystem = new EntityCountingSystem(world);
            var commandSystem = new CommandSystem(world);

            var _updateSystems = new SequentialSystem<float>([
                mousePollingSystem,
                entityCountingSystem,
                commandSystem,
                ]);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            //initialize the systems (draw)
            var entityRenderSystem = new EntityRenderSystem(world);
            var tileMapRenderSystem = new TileMapRenderSystem(world, _assetManager);

            var _drawSystems = new SequentialSystem<SpriteBatch>([
                entityRenderSystem,
                tileMapRenderSystem
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