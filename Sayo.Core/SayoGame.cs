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

            _graphicsDeviceManager.PreferredBackBufferWidth = 640;
            _graphicsDeviceManager.PreferredBackBufferHeight = 640;
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
            SoundManager.Initialize(Content);
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
            // 初始化 Gum 服务。第二个参数指定默认视觉版本，V2 是最新版本。
            GumService.Default.Initialize(this, DefaultVisualsVersion.V2);

            // 告诉 Gum 服务使用哪个 ContentManager。这里使用 Core 中的全局 ContentManager。
            GumService.Default.ContentLoader!.XnaContentManager = Content;

            // 注册键盘输入，用于 UI 控制。
            FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);

            // 注册手柄输入，用于 UI 控制。
            FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);
            
            // 自定义“反向 Tab”导航，让键盘 ↑ 也能触发。
            FrameworkElement.TabReverseKeyCombos.Add(
                new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up });

            // 自定义 Tab 导航，让键盘 ↓ 也能触发。
            FrameworkElement.TabKeyCombos.Add(
                new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down });

            // UI 资源是按原尺寸的 1/4 制作的，以保持图集小。
            // 因此设置 Gum 默认画布为显示分辨率的 1/4，然后把 Gum 摄像机放大 4 倍。
            GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
            GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
            GumService.Default.Renderer.Camera.Zoom = 4.0f;
        }
    }
}