using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using System;
namespace Sayo.Core.Scene
{
    internal class SettingScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        private Panel _gamePanel;
        public override void Load()
        {
            CreatePanel();

        }
        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.Clear(Color.Tomato);

            GumService.Default.Draw();
        }
        public override void Update(GameTime gameTime)
        {
            //TODO;
        }
        public override void Unload()
        {
            SB.Dispose();
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
