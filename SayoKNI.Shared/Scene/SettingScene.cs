using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using SayoKNI;
using System;
namespace Sayo.Core.Scene
{
    internal class SettingScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        private Panel _gamePanel;
        private SpriteFont _font;
        private int _windowWidth;
        private int _windowHeight;

        public override void Load()
        {
            _font ??= TextureManager.Font;
            _windowWidth = GameGraphicsDevice.Viewport.Width;
            _windowHeight = GameGraphicsDevice.Viewport.Height;
            CreatePanel();
        }
        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.Clear(Color.Tomato);
            SB.Begin();
            float _windowWidthx = _windowWidth / 2 - _font.MeasureString(_windowWidth.ToString()).X / 2;
            float _windowHeightx = _windowWidth / 2 - _font.MeasureString(_windowHeight.ToString()).X / 2;
            float middleY = _windowHeight / 2;
            SB.DrawString(_font, _windowWidth.ToString(), new Vector2(_windowWidthx, middleY - 100), Color.White);
            SB.DrawString(_font, _windowHeight.ToString(), new Vector2(_windowHeightx, middleY + 100), Color.White);
            SB.End();
            GumService.Default.Draw();
        }
        public override void Update(GameTime gameTime)
        {
            _windowWidth = GameGraphicsDevice.Viewport.Width;
            _windowHeight = GameGraphicsDevice.Viewport.Height;
            _windowWidth = GameGraphicsDevice.Viewport.Width;
            GumService.Default.CanvasWidth = GameGraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
            GumService.Default.CanvasHeight = GameGraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
            GumService.Default.Update(gameTime);
        }
        public override void Unload()
        {
        }
        private void CreatePanel()
        {
            GumService.Default.Root.Children?.Clear();
            // Create a container to hold all of our buttons
            _gamePanel = new Panel();
            _gamePanel.Dock(Dock.Fill);
            _gamePanel.AddToRoot();

            Helper.CreateButton(_gamePanel, BackButton_Click, "back", Anchor.BottomRight, width: 27, height: 5, textscale: 0.4f);
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            SceneManager.ChangeScene("MainMenu");
        }
    }
}
