using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Input;
using Sayo.Core.Content.obj;

namespace Sayo.Core
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings, 
    /// and platform-specific configurations.
    /// </summary>
    public class SayoGame : Game
    {
        // Resources for drawing.
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        SpriteBatch _sb;

        /// <summary>
        /// Initializes a new instance of the game. Configures platform-specific settings, 
        /// initializes services like settings and leaderboard managers, and sets up the 
        /// screen manager for screen transitions.
        /// </summary>
        public SayoGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            // Share GraphicsDeviceManager as a service.
            Services.AddService(typeof(GraphicsDeviceManager), graphicsDeviceManager);

            graphicsDeviceManager.PreferredBackBufferWidth = 1280;
            graphicsDeviceManager.PreferredBackBufferHeight = 720;
            graphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";

            // Configure screen orientations.
            graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }
        /// <summary>
        /// Initializes the game, including setting up localization and adding the 
        /// initial screens to the ScreenManager.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Load supported languages and set the default language.
            List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
            var languages = new List<CultureInfo>();
            for (int i = 0; i < cultures.Count; i++)
            {
                languages.Add(cultures[i]);
            }

            // TODO You should load this from a settings file or similar,
            // based on what the user or operating system selected.
            var selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
            LocalizationManager.SetCulture(selectedLanguage);
        }

        private SayoPlayer _sayo;
        private Grid _grid;
        public static Food Food;
        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            _sb = new SpriteBatch(GraphicsDevice);

            var head = Content.Load<Texture2D>("SayoHead");
            var head_eatting = Content.Load<Texture2D>("SayoHead_Eating");
            var body = Content.Load<Texture2D>("SayoBody");
            var body_full = Content.Load<Texture2D>("SayoBody_Full");
            var body_turn = Content.Load<Texture2D>("SayoBody_Turn");
            var body_turn_full = Content.Load<Texture2D>("SayoBody_Turn_Full");
            var bodys = new [] { body, body_full, body_turn, body_turn_full };
            var heads = new [] { head, head_eatting };
            var butt = Content.Load<Texture2D>("SayoButt");
            var food = Content.Load<Texture2D>("Food");

            _grid = new Grid();
            _sayo = new SayoPlayer(heads,bodys,butt,_grid);
            Food = new Food(food);
            Food.Update(_grid);
        }

        private TimeSpan _moveTimer = TimeSpan.Zero;
        private readonly TimeSpan _moveInterval = TimeSpan.FromSeconds(0.25);
        private Keys lastKey = Keys.None;
        public static bool GameRunning = true;

        /// <summary>
        /// Updates the game's logic, called once per frame.
        /// 获取键盘状态,在tick时将最后一次输入传入sayo的update中
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for game updates.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _moveTimer += gameTime.ElapsedGameTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                lastKey = Keys.Up;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                lastKey = Keys.Down;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                lastKey = Keys.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                lastKey = Keys.Left;
            }

            if (_moveTimer < _moveInterval) return;


            _moveTimer = TimeSpan.Zero;
            if (GameRunning)
            {
                _sayo.Update(lastKey, _grid);
            }

            lastKey = Keys.None;
            base.Update(gameTime);
        }

        public static void GameOver()
        {
            GameRunning = false;
            //TODO;
        }
        /// <summary>
        /// Draws the game's graphics, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for rendering.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the MonoGame orange color before drawing.
            GraphicsDevice.Clear(Color.MonoGameOrange);

            _sb.Begin();
            _grid.Draw(_sb);
            _sb.End();

            base.Draw(gameTime);
        }
    }
}