using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sayo.Core.Scene
{
    public abstract class SceneBase(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
    {
        public SpriteBatch SB = new(graphicsDevice);
        public GraphicsDevice GraphicsDevice = graphicsDevice;
        public ContentManager Content = content;
        public GraphicsDeviceManager GraphicsDeviceManager = graphicsDeviceManager;

        abstract public void Load();
        abstract public void Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime);
        abstract public void Unload();
    }
}