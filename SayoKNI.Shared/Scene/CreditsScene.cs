using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameGum;
using System;

namespace Sayo.Core.Scene
{
    internal class CreditsScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        SpriteFont _font;
        private int _windowWidth;

        // 滚动偏移量
        private float _scrollOffset = 0;

        // 拖拽相关
        private bool _dragging = false;
        private int _lastMouseY;

        private Panel _gamePanel;

        public override void Load()
        {
            _font = Content.Load<SpriteFont>("Fonts/Hud");
            _windowWidth = GameGraphicsDevice.Viewport.Width;
            CreatePanel();
        }

        public override void Update(GameTime gameTime)
        {
            GumService.Default.Update(gameTime);
            UpdateScrollInput();
        }

        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.Clear(Color.Tomato);
            SB.Begin();

            for (int i = 0; i < credits.Length; i++)
            {
                var text = credits[i];
                float x = _windowWidth / 2 - _font.MeasureString(text).X / 2;
                float y = 100 + i * 40 + _scrollOffset;

                SB.DrawString(_font, text, new Vector2(x, y), Color.White);
            }

            SB.End();
            GumService.Default.Draw();
        }

        public override void Unload()
        {

        }


        private void UpdateScrollInput()
        {
            var mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (!_dragging)
                {
                    _dragging = true;
                    _lastMouseY = mouse.Y;
                }
                else
                {
                    int delta = mouse.Y - _lastMouseY;
                    _scrollOffset += delta;
                    _lastMouseY = mouse.Y;
                }
            }
            else
            {
                _dragging = false;
            }
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

        readonly string[] credits =
        [
            "Design",
            "君临德雷克",
            " ",
            "Program",
            "Jirigalang",
            " ",
            "Artist",
            "LongBaTian(enshi tutou)",
            " ",
            "Character",
            "Sayo © 原版权方",
            " ",
            "Powered by MonoGame",
            "Thank you for playing!"
        ];
    }
}
