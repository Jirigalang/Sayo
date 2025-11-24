using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using System;

namespace Sayo.Core.Scene;
internal class GameOverScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
{
    private Panel _GameOverButtonsPanel;
    SpriteFont _font;
    private int _windowWidth;
    private int _windowHeight;
    public override void Load()
    {
        _font = Content.Load<SpriteFont>("Fonts/Hud");
        _windowWidth = GraphicsDevice.Viewport.Width;
        _windowHeight = GraphicsDevice.Viewport.Height;
        CreatePanel();
    }

    public override void Draw(GameTime gameTime)
    {
        //不调用GraphicsDevice.Clear(),以保留游戏画面作为背景
        if (_GameOverButtonsPanel.IsVisible)
        {
            string message = "Game Over";
            string message2 = $"总分:{Helper.Score}";
            var picSize = _font.MeasureString(message);
            var picSize2 = _font.MeasureString(message2);
            var position1 = new Vector2((_windowWidth - picSize.X) / 2f, (_windowHeight - picSize.Y) / 2 - 50);
            var position2 = new Vector2((_windowWidth - picSize2.X) / 2f, (_windowHeight - picSize2.Y) / 2 + 50);
            SB.Begin(samplerState: SamplerState.PointClamp);
            SB.DrawString(_font, message, position1, Color.Brown);
            SB.DrawString(_font, message2, position2, Color.Brown);
            SB.End();
            GumService.Default.Draw();
        }
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
    }

    public override void Unload()
    {
        SB.Dispose();
    }

    private void CreatePanel()
    {
        GumService.Default.Root.Children.Clear();
        // Create a container to hold all of our buttons
        _GameOverButtonsPanel = new Panel();
        _GameOverButtonsPanel.Dock(Dock.Fill);
        _GameOverButtonsPanel.AddToRoot();


        var retryButton = Helper.CreateButton(_GameOverButtonsPanel, HandleRetryClicked, "重试", Anchor.Bottom, width: 70);

        retryButton.IsFocused = true;
    }

    private void HandleRetryClicked(object sender, EventArgs e)
    {
        SceneManager.ChangeScene("Game");
    }
}