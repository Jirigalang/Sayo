using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sayo.Core.Scene;
internal class GameOverScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
{
    SpriteFont _font;
    private int _windowWidth;
    private int _windowHeight;
    public override void Load()
    {
        _font = Content.Load<SpriteFont>("Fonts/Hud");
        _windowWidth = GraphicsDevice.Viewport.Width;
        _windowHeight = GraphicsDevice.Viewport.Height;
    }

    public override void Draw(GameTime gameTime)
    {
        //不调用GraphicsDevice.Clear(),以保留游戏画面作为背景
        string message = "Game Over";
        string message2 = "直接退出吧做一个按钮老麻烦了";
        var picSize = _font.MeasureString(message);
        var picSize2 = _font.MeasureString(message2);
        var position1 = new Vector2((_windowWidth - picSize.X) / 2f, (_windowHeight - picSize.Y) / 2 - 100);
        var position2 = new Vector2((_windowWidth - picSize2.X) / 2f, (_windowHeight - picSize2.Y) / 2 + 100);
        SB.Begin();
        SB.DrawString(_font, message, position1, Color.Brown);
        SB.DrawString(_font, message2, position2, Color.Brown);
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
}