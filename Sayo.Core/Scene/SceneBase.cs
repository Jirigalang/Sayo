using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core.Content.obj;

namespace Sayo.Core.Scene
{
    public abstract class SceneBase
    {
        public SpriteBatch _sb;

        abstract public void Load(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager);
        abstract public void Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime);
        abstract public void Unload(ContentManager content);
    }
}