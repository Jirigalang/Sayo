using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Input;
using Sayo.Core.Content.obj;
using Sayo.Core.Scene;

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
        private SpriteBatch _sb;

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

        
        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            _sb = new SpriteBatch(GraphicsDevice);
            GameScene gameScene = new GameScene();
        }



        /// <summary>
        /// Updates the game's logic, called once per frame.
        /// 获取键盘状态,在tick时将最后一次输入传入sayo的update中
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for game updates.
        /// </param>
        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public static void GameOver()
        {
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



            base.Draw(gameTime);
        }
    }
}