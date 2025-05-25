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
            _context = InitGameContext();
            
            _inputManager = new InputManager<PlayerAction>()
                .BindAction(Keys.W, PlayerAction.Jump)
                .BindAction(Keys.S, PlayerAction.Crouch)
                .BindAction(Keys.D, PlayerAction.MoveRight)
                .BindAction(Keys.A, PlayerAction.MoveLeft);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            //sets up the containers for DefaultEcs
            var world = new World();
            var inputBridge = new InputBridge(_inputManager, world);

            //initializes the systems
            var entityCountingSystem = new EntityCountingSystem(_context);
            var commandSystem = new CommandSystem(_context);

            //FOR NOW: use magic number (32), this should be moved somewhere later
            var entityRenderSystem = new EntityRenderSystem(_context, 32);
            
            var _updateSystems = new SequentialSystem<float>([
                entityCountingSystem,
                commandSystem,
                ]);

            var _drawSystems = new SequentialSystem<SpriteBatch>([
                entityRenderSystem
                ]);


            //send back the initialized GameContext
            return new GameContext(
                world: world,
                inputBridge: inputBridge,
                onUpd: _updateSystems,
                onDraw: _drawSystems
                );
        }
        
    }
}