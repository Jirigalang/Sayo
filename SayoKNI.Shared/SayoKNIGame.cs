using Gum.Forms;
using Gum.Forms.Controls;
using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core;
using System;

namespace SayoKNI;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class SayoKNIGame : Game
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    public static float DRP { get; set; } = 1.0f;
    public static ImGuiRenderer ImGuiRenderer { get; private set; }

    public SayoKNIGame(double drp = 1f)
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        // Share _graphicsDeviceManager as a service.
        Services.AddService(_graphicsDeviceManager);
        Content.RootDirectory = "Content";
        _graphicsDeviceManager.GraphicsProfile = GraphicsProfile.FL10_0;
        _graphicsDeviceManager.PreferredBackBufferWidth = 960;
        _graphicsDeviceManager.PreferredBackBufferHeight = 960;
        IsMouseVisible = true;
        _graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        DRP = (float)drp;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        Window.ClientSizeChanged += OnClientSizeChanged;
        Window.AllowUserResizing = true;
        // 创建 ImGui 渲染器。
        ImGuiRenderer = new ImGuiRenderer(this);
        ImGuiRenderer.RebuildFontAtlas();
        base.Initialize();

    }

    private void OnClientSizeChanged(object sender, EventArgs e)
    {
        Gum.GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
        Gum.GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        base.LoadContent();
        InitializeGum();
        SceneManager.Initialize(GraphicsDevice, Content, _graphicsDeviceManager);
        TextureManager.Initialize(Content);
        SceneManager.ChangeScene("MainMenu");
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
#if DEBUG
        // 注册游戏场景和时间信息到Debug管理器
        if (SceneManager.CurrentScene is Sayo.Core.Scene.GameScene gameScene)
        {
            ImGuiDebugManager.Instance.RegisterGameScene(gameScene);
            ImGuiDebugManager.Instance.RegisterGameTime(gameTime);
        }

        // 按F1切换调试面板
        var keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        if (keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
        {
            if (!_lastFrameF1Pressed)
            {
                ImGuiDebugManager.Instance.ToggleVisibility();
                _lastFrameF1Pressed = true;
            }
        }
        else
        {
            _lastFrameF1Pressed = false;
        }
#endif

        SceneManager.CurrentScene.Update(gameTime);
        SoundManager.Update(gameTime);
        base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        SceneManager.CurrentScene.Draw(gameTime);
        ImGuiRenderer.BeforeLayout(gameTime);

#if DEBUG
        ImGuiDebugManager.Instance.Render();
#endif

        // 在此结束调试 UI 的绘制
        ImGuiRenderer.AfterLayout();
        base.Draw(gameTime);
    }

    private bool _lastFrameF1Pressed = false;
    private void InitializeGum()
    {
        // 初始化 Gum 服务。第二个参数指定默认视觉版本，V3 是最新版本。
        Gum.GumService.Default.Initialize(this, DefaultVisualsVersion.V3);

        // 注册键盘输入，用于 UI 控制。
        FrameworkElement.KeyboardsForUiControl.Add(Gum.GumService.Default.Keyboard);

        // 注册手柄输入，用于 UI 控制。
        FrameworkElement.GamePadsForUiControl.AddRange(Gum.GumService.Default.Gamepads);

        // 自定义“反向 Tab”导航，让键盘 ↑ 也能触发。
        FrameworkElement.TabReverseKeyCombos.Add(
            new KeyCombo() { PushedKey = Gum.Forms.Input.Keys.Up });

        // 自定义 Tab 导航，让键盘 ↓ 也能触发。
        FrameworkElement.TabKeyCombos.Add(
            new KeyCombo() { PushedKey = Gum.Forms.Input.Keys.Down });

        // UI 资源是按原尺寸的 1/4 制作的，以保持图集小。
        // 因此设置 Gum 默认画布为显示分辨率的 1/4，然后把 Gum 摄像机放大 4 倍。
        Gum.GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
        Gum.GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
        Gum.GumService.Default.Renderer.Camera.Zoom = 4.0f;
    }
}

public enum Platform
{
    Windows,
    Android,
    iOS,
    DesktopGL,
    Web
}