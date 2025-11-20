using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using System;
using Gum.Wireframe;
using Gum.Forms.DefaultVisuals;

namespace Sayo.Core.Scene
{
    internal class MainMenuScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager),IDisposable
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
            InitializeUI();
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
        public void Dispose()
        {
            SB.Dispose();
        }
        private void CreateTitlePanel()
        {
            // Create a container to hold all of our buttons
            _titleScreenButtonsPanel = new Panel();
            _titleScreenButtonsPanel.Dock(Dock.Fill);
            _titleScreenButtonsPanel.AddToRoot();

            
            var startButton = new Button();
            startButton.Anchor(Anchor.Bottom);
            startButton.Visual.Y = -12;
            startButton.Visual.Width = 70;
            startButton.Text = "Start";
            startButton.Click += HandleStartClicked;
            _titleScreenButtonsPanel.AddChild(startButton);

            var settingButton = new Button();
            settingButton.Anchor(Anchor.BottomRight);
            settingButton.Text = "Setting";
            var visual = (ButtonVisual)settingButton.Visual;
            visual.Width = 23;
            visual.Height = 5;
            var textInstance = visual.TextInstance;
            textInstance.CustomFontFile = "fonts/04b_30.fnt";
            visual.TextInstance.FontSize = 5;
            visual.TextInstance.FontScale = 0.4f;
            settingButton.Click += SettingButton_Click;
            _titleScreenButtonsPanel.AddChild(settingButton);

            var creditsButton = new Button();
            creditsButton.Anchor(Anchor.BottomRight);
            visual = (ButtonVisual)creditsButton.Visual;
            visual.Width = 23;
            visual.Height = 5;
            visual.Y = -20;
            visual.TextInstance.FontSize = 5;
            visual.TextInstance.FontScale = 0.4f;
            creditsButton.Text = "Credits";
            creditsButton.Click += CreditsButton_Click;
            _titleScreenButtonsPanel.AddChild(creditsButton);

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

        private void InitializeUI()
        {
            // Clear out any previous UI in case we came here from
            // a different screen:
            GumService.Default.Root.Children.Clear();

            CreateTitlePanel();
        }
    }
}