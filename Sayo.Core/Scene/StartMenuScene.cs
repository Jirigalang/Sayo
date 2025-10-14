using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
namespace Sayo.Core.Scene
{
    internal class StartMenuScene : SceneBase
    {
        SpriteFont _font;
        private int _windowWidth;
        public override void Load(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager GraphicsDeviceManager)
        {
            _sb = new(graphicsDevice);
            _font = content.Load<SpriteFont>("Fonts/Hud");
            _windowWidth = graphicsDevice.Viewport.Width;
        }
        public override void Draw(GameTime gameTime)
        {
            _sb.Begin();
            string message = "按任意键开始";
            _sb.DrawString(_font, message, new Vector2(_windowWidth / 2 - _font.MeasureString(message).X / 2, 600), Color.White);
            _sb.End();
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().GetPressedKeyCount() > 0
                || Mouse.GetState().LeftButton == ButtonState.Pressed
                || TouchPanel.GetState().Count > 0)
            {
                SceneManager.ChangeScene(new GameScene());
            }

        }
        public override void Unload(ContentManager content)
        {
            _sb.Dispose();
        }
    }
}
