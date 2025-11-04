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
        //不调用GraphicsDevice.Clear(),以保留游戏画面作为背景
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