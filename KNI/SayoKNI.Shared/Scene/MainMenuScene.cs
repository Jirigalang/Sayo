using System;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace Sayo.Core.Scene;

internal class MainMenuScene(
    GraphicsDevice graphicsDevice,
    ContentManager content,
    GraphicsDeviceManager graphicsDeviceManager)
    : SceneBase(graphicsDevice, content, graphicsDeviceManager)
{
    private Panel _titleScreenButtonsPanel;
    private int _windowHeight;
    private int _windowWidth;
    private Texture2D title;

    public override void Load()
    {
        title = Content.Load<Texture2D>("SayoTitle");
        CreateTitlePanel();
        SoundManager.SEList[SEName.Opening].Play();
    }

    public override void Draw(GameTime gameTime)
    {
        GameGraphicsDevice.Clear(Color.Tomato);
        if (_titleScreenButtonsPanel.IsVisible)
        {
            var picSize = title.Bounds.Size.ToVector2();
            SB.Begin(samplerState: SamplerState.PointClamp);
            var position = new Vector2((_windowWidth - picSize.X) / 2f,
                                       (_windowHeight - picSize.Y) / 2f - 100);
            SB.Draw(title, position, Color.White);
            SB.End();
        }

        GumService.Default.Draw();
    }

    public override void Update(GameTime gameTime)
    {
        _windowWidth = GameGraphicsDevice.Viewport.Width;
        _windowHeight = GameGraphicsDevice.Viewport.Height;
        GumService.Default.Update(gameTime);
    }

    public override void Unload()
    {
    }

    private void CreateTitlePanel()
    {
        GumService.Default.Root.Children?.Clear();
        // Create a container to hold all of our buttons
        _titleScreenButtonsPanel = new Panel();
        _titleScreenButtonsPanel.Dock(Dock.Fill);
        _titleScreenButtonsPanel.AddToRoot();

        var startButton = Helper.CreateButton(_titleScreenButtonsPanel, HandleStartClicked, "start", Anchor.Bottom, width: 70);
        var settingButton = Helper.CreateButton(_titleScreenButtonsPanel, SettingButton_Click, "setting", Anchor.BottomRight,
            45);
        var creditsButton = Helper.CreateButton(_titleScreenButtonsPanel, CreditsButton_Click, "staff",
            Anchor.BottomRight,
            30, 5, 0.4f, 0, -22);

        startButton.IsFocused = true;
    }

    private static void CreditsButton_Click(object sender, EventArgs e)
    {
        SceneManager.ChangeScene("Credits");
    }

    private void SettingButton_Click(object sender, EventArgs e)
    {
        SceneManager.ChangeScene("Setting");
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {
        // Change to the game scene to start the game.
        SceneManager.ChangeScene("Game");
    }
}