using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace Sayo.Core.Scene
{
    internal class SettingScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        public override void Load()
        {
            //TODO;
        }
        public override void Draw(GameTime gameTime)
        {
            //TODO;
        }
        public override void Update(GameTime gameTime)
        {
            //TODO;
        }
        public override void Unload()
        {
            SB.Dispose();
        }
    }
}
