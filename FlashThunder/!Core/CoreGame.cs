using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FlashThunder.Enums;
using FlashThunder.Managers;
using MonoGameGum;
using FlashThunder.States;
using FlashThunder.Defs;
using FlashThunder.Factories;

namespace FlashThunder.Core
{
    public class CoreGame : Game
    {

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // TODO: This should just be a menuAction input manager.
        // The game input manager should only exist within the game runtime
        private InputManager<GameAction> _gameInputMngr;

        // TODO: Same for this. We should still ahve an assetmanager for the menu tho.
        // Actually think about this abit because we don't want to reload textures all the time
        private TexManager _texMngr;
        private TileManager _tileMngr;
        private UIManager _uiMngr;
        private StateManager _stateMngr;
        private EventBus _higherEventBus;


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
            _higherEventBus = new EventBus();

            _gameInputMngr = new InputManager<GameAction>()
                .BindAction(Keys.W, GameAction.MoveUp)
                .BindAction(Keys.S, GameAction.MoveDown)
                .BindAction(Keys.D, GameAction.MoveRight)
                .BindAction(Keys.A, GameAction.MoveLeft)
                .BindAction(Keys.LeftShift, GameAction.SpeedUpCamera)
                .BindAction(Keys.O, GameAction.SpawnTest);

            _texMngr = new TexManager(Content, "clearTile");
            _tileMngr = new TileManager();
            _stateMngr = new StateManager(_higherEventBus);

            _uiMngr = new UIManager(_higherEventBus)
                .SetupListeners(Window)
                .RescaleUIToResolution(Window);

            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texMngr
                .LoadDefinitions("texture_manifest.json");
            _tileMngr
                .LoadDefinitions(_texMngr, "tile_defs.json");
            _stateMngr
                .Register(typeof(GameRunningState), new GameFactory
                (_higherEventBus, _gameInputMngr, _texMngr, _tileMngr).Create)
                .Register(typeof(TitleState), () => new TitleState(_higherEventBus))
                .SwitchTo(typeof(TitleState));
        }

        protected override void Update(GameTime gameTime)
        {
            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // - - - [ Higher system updates ] - - -
            _gameInputMngr.Update();
            _stateMngr.Update(dt);
            _uiMngr.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // - - - [ Spritebatch begins here ] - - -
            _stateMngr.Draw(_spriteBatch);

            // -- [ Spritebatch ends here ] - - -
            _spriteBatch.End();

            _uiMngr.Draw();
            base.Draw(gameTime);
        }
    }
}