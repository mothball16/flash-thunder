using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FlashThunder.Enums;
using FlashThunder.Managers;
using MonoGameGum;
using FlashThunder.States;
using FlashThunder.Defs;

namespace FlashThunder.Core
{
    public class CoreGame : Game
    {

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // TODO: This should just be a menuAction input manager.
        // The game input manager should only exist within the game runtime
        private InputManager<GameAction> _gameInputManager;

        // TODO: Same for this. We should still ahve an assetmanager for the menu tho.
        // Actually think about this abit because we don't want to reload textures all the time
        private TexManager _texManager;
        private TileManager _tileManager;
        private UIManager _uiManager;
        private StateManager _stateManager;


        public CoreGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // - - - [ Initialize higher systems ] - - -
            GumService.Default.Initialize(this, AssetPaths.UIProj);

            _gameInputManager = new InputManager<GameAction>()
                .BindAction(Keys.W, GameAction.MoveUp)
                .BindAction(Keys.S, GameAction.MoveDown)
                .BindAction(Keys.D, GameAction.MoveRight)
                .BindAction(Keys.A, GameAction.MoveLeft)
                .BindAction(Keys.LeftShift, GameAction.SpeedUpCamera)
                .BindAction(Keys.O, GameAction.SpawnTest);

            _texManager = new TexManager(Content, "clearTile");
            _tileManager = new TileManager();
            _stateManager = new StateManager();

            _uiManager = new UIManager()
                .SetupListeners(Window);

            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texManager
                .LoadDefinitions("texture_manifest.json");
            _tileManager
                .LoadDefinitions(_texManager, "tile_defs.json");
            _stateManager
                .Register(new GameRunningState(_texManager, _gameInputManager, _tileManager))
                .Register(new MenuState())
                .SwitchTo(typeof(MenuState));
        }

        protected override void Update(GameTime gameTime)
        {
            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // - - - [ Higher system updates ] - - -
            _gameInputManager.Update();
            _stateManager.Update(dt);
            _uiManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // - - - [ Spritebatch begins here ] - - -
            _stateManager.Draw(_spriteBatch);
            _spriteBatch.End();

            // -- [ Spritebatch ends here ] - - -
            _uiManager.Draw();
            base.Draw(gameTime);
        }
    }
}