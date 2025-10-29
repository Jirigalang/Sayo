using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sayo.Core.Scene;
internal class GameOverScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
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