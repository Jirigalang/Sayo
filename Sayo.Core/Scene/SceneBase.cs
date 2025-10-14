using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core.Content.obj;

namespace Sayo.Core.Scene
{
    public abstract class SceneBase
    {
        abstract public void Load(SpriteBatch sb, ContentManager content, GraphicsDeviceManager GraphicsDeviceManager);
        abstract public void Update(GameTime gameTime);
        abstract public void Draw();
        abstract public void Unload(ContentManager content);
    }
}