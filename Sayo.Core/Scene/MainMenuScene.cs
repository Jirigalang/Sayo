using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using System;

namespace Sayo.Core.Scene
{
    internal class MainMenuScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        SpriteFont _font;
        Texture2D title;
        private int _windowWidth;
        private int _windowHeight;
        private Panel _titleScreenButtonsPanel;
        public override void Load()
        {
            _font = Content.Load<SpriteFont>("Fonts/Hud");

            _windowWidth = GraphicsDevice.Viewport.Width;
            _windowHeight = GraphicsDevice.Viewport.Height;
            title = Content.Load<Texture2D>("SayoTitle");
            CreateTitlePanel();
            SoundManager.SEList[SEName.Opening].Play();
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Tomato);
            if (_titleScreenButtonsPanel.IsVisible)
            {
                var picSize = title.Bounds.Size.ToVector2();
                SB.Begin(samplerState: SamplerState.PointClamp);
                var position = new Vector2((_windowWidth - picSize.X) / 2f, (_windowHeight - picSize.Y) / 2 - 100);
                SB.Draw(title, position, Color.White);
                SB.End();
            }

            GumService.Default.Draw();
        }
        public override void Update(GameTime gameTime)
        {
            GumService.Default.Update(gameTime);
        }
        public override void Unload()
        {
            SB.Dispose();
        }
        private void CreateTitlePanel()
        {
            GumService.Default.Root.Children.Clear();
            // Create a container to hold all of our buttons
            _titleScreenButtonsPanel = new Panel();
            _titleScreenButtonsPanel.Dock(Dock.Fill);
            _titleScreenButtonsPanel.AddToRoot();


            var startButton = Helper.CreateButton(_titleScreenButtonsPanel, HandleStartClicked, "开始", Anchor.Bottom, horizontalOffect: -12, width: 70);
            var settingButton = Helper.CreateButton(_titleScreenButtonsPanel, SettingButton_Click, "设置", Anchor.BottomRight, width: 23, height: 5, textscale: 0.4f);

            var creditsButton = new Button();
            var creditsButtton = Helper.CreateButton(_titleScreenButtonsPanel, CreditsButton_Click, "制作人员名单", Anchor.BottomRight,
                width: 23, height: 5, textscale: 0.4f, horizontalOffect: -0, longitudinalOffset: -20);

            startButton.IsFocused = true;
        }

        private void CreditsButton_Click(object sender, EventArgs e)
        {
            SceneManager.ChangeScene("Credits");
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            var visual = (ButtonVisual)_titleScreenButtonsPanel.Children[1].Visual;
            MessageBox.Show("message", $"FontScale:{visual.TextInstance.FontSize}", ["OK"]);
            //SceneManager.ChangeScene("Setting");
        }

        private void HandleStartClicked(object sender, EventArgs e)
        {
            // Change to the game scene to start the game.
            SceneManager.ChangeScene("Game");
        }
    }
}