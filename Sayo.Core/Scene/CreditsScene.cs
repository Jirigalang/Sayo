using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sayo.Core.Content.obj;
using Microsoft.Xna.Framework;
namespace Sayo.Core.Scene
{
    internal class CreditsScene : SceneBase
    {
        SpriteFont _font;
        private int _windowWidth;
        public override void Load(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        {
            _sb = new(graphicsDevice);
            _font = content.Load<SpriteFont>("Font");
            _windowWidth = graphicsDevice.Viewport.Width;
        }
        public override void Draw(GameTime gameTime)
        {
            _sb.Begin();
            for(int i = 0; i < credits.Length; i++)
            {
                _sb.DrawString(_font, credits[i], new Vector2(_windowWidth / 2 - _font.MeasureString(credits[i]).X / 2, 100 + i * 40), Color.White);
            }
            
            _sb.End();
        }
        public override void Update(GameTime gameTime)
        {
            //TODO;
        }
        public override void Unload(ContentManager content)
        {
            _sb.Dispose();
        }
        readonly string[] credits =
        [
            "Design",
            "君临德雷克",
            "Program",
            "Jirigalang",
            "Artist",
            "龙霸天6324",
            "Powered by MonoGame",
            "Thank you for playing!"
        ];
    }
}
