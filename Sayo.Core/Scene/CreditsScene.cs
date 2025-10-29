using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
// ReSharper disable All
namespace Sayo.Core.Scene
{
    internal class CreditsScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        SpriteFont _font;
        private int _windowWidth;
        public override void Load()
        {
            _font = Content.Load<SpriteFont>("Fonts/Hud");
            _windowWidth = GraphicsDevice.Viewport.Width;
        }
        public override void Draw(GameTime gameTime)
        {
            SB.Begin();
            for(int i = 0; i < credits.Length; i++)
            {
                SB.DrawString(_font, credits[i], new Vector2(_windowWidth / 2 - _font.MeasureString(credits[i]).X / 2, 100 + i * 40), Color.White);
            }
            
            SB.End();
        }
        public override void Update(GameTime gameTime)
        {
            //TODO;
        }
        public override void Unload()
        {
            SB.Dispose();
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
            "Powered by MonoGame",
            "Thank you for playing!"
        ];
    }
}
