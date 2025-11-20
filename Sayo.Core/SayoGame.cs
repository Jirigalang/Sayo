using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using MonoGameGum;
using Sayo.Core.Localization;

namespace Sayo.Core
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings, 
    /// and platform-specific configurations.
    /// </summary>
    public class SayoGame : Game
    {
        // Resources for drawing.
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        /// <summary>
        /// Initializes a new instance of the game. Configures platform-specific settings, 
        /// initializes services like settings and leaderboard managers, and sets up the 
        /// screen manager for screen transitions.
        /// </summary>
        public SayoGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            // Share _graphicsDeviceManager as a service.
            Services.AddService(typeof(GraphicsDeviceManager), _graphicsDeviceManager);

            _graphicsDeviceManager.PreferredBackBufferWidth = 1280;
            _graphicsDeviceManager.PreferredBackBufferHeight = 720;
            _graphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";
            
            // Configure screen orientations.
            _graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }
        /// <summary>
        /// Initializes the game, including setting up localization and adding the 
        /// initial screens to the ScreenManager.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            
            // based on what the user or operating system selected.
            const string selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
            LocalizationManager.SetCulture(selectedLanguage);
            IsMouseVisible = true;
        }


        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            InitializeGum();
            SceneManager.Initialize(GraphicsDevice, Content, _graphicsDeviceManager);
            SceneManager.ChangeScene("MainMenu");
        }
        /// <summary>
        /// Updates the game's logic, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for game updates.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            SceneManager.CurrentScene.Update(gameTime);
            SoundManager.Update(gameTime);
            base.Update(gameTime);
        }


        /// <summary>
        /// Draws the game's graphics, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for rendering.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            SceneManager.CurrentScene.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void InitializeGum()
        {
            // Initialize the Gum service. The second parameter specifies
            // the version of the default visuals to use. V2 is the latest
            // version.
            GumService.Default.Initialize(this, DefaultVisualsVersion.V2);

            // Tell the Gum service which content manager to use.  We will tell it to
            // use the global content manager from our Core.
            GumService.Default.ContentLoader!.XnaContentManager = Content;

            // Register keyboard input for UI control.
            FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);

            // Register gamepad input for Ui control.
            FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);

            // Customize the tab reverse UI navigation to also trigger when the keyboard
            // Up arrow key is pushed.
            FrameworkElement.TabReverseKeyCombos.Add(
                new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up });

            // Customize the tab UI navigation to also trigger when the keyboard
            // Down arrow key is pushed.
            FrameworkElement.TabKeyCombos.Add(
                new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down });

            // The assets created for the UI were done so at 1/4th the size to keep the size of the
            // texture atlas small.  So we will set the default canvas size to be 1/4th the size of
            // the game's resolution then tell gum to zoom in by a factor of 4.
            GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
            GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
            GumService.Default.Renderer.Camera.Zoom = 4.0f;
        }
    }
}