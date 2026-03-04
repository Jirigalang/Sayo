using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SayoKNI;
using System;

namespace Sayo.Core.Object;

/// <summary>
/// 表示用于管理和渲染精灵的二维网格结构。
/// </summary>
/// <remarks>
/// <see cref="Grid"/> 类提供了初始化、操作和呈现
/// 精灵网格。它支持在特定位置设置精灵、在精灵之间移动精灵等操作
/// 并将网格渲染到目标。网格尺寸和单元格大小是可配置的。
/// </remarks>
public class Grid
{
    private readonly int _row = 10;
    private readonly int _column = 10;
    public readonly Sprite[,] Cell;
    private const int _cellWidth = 64;
    public int CellWidth { get { return _cellWidth / (int)SayoKNIGame.DRP; } }
    private Vector2 Zero = Vector2.Zero;
    public RenderTarget2D Map;
    private static GraphicsDevice _graphicsDevice;
    private Texture2D _tile;
    public Grid(int row, int column)
    {
        Cell = new Sprite[row, column];
        _row = row;
        _column = column;
    }
    public Grid() { }
    public void Initialize(SpriteBatch sb, Texture2D tile, GraphicsDeviceManager graphicsDeviceManager = null)
    {
        _graphicsDevice ??= graphicsDeviceManager.GraphicsDevice;
        _tile = tile;
        var map = new RenderTarget2D(_graphicsDevice, _row * CellWidth, _column * CellWidth);
        _graphicsDevice.SetRenderTarget(map);
        _graphicsDevice.Clear(Color.Transparent);
        sb.Begin();
        for (int y = 0; y < _column; y++)
            for (int x = 0; x < _row; x++)
            {
                Vector2 position = new()
                {
                    X = x * CellWidth,
                    Y = y * CellWidth
                };
                sb.Draw(_tile, position, Color.White);
            }
        sb.End();
        _graphicsDevice.SetRenderTarget(null);
        Map = map;
    }
    public void Move(Point sourecPosition, Point targetPosition)
    {
        if (sourecPosition == targetPosition)
            throw new Exception("源位置与目标位置相同, 无需移动");
        if (Cell[sourecPosition.X, sourecPosition.Y] is null)
            throw new Exception("源位置无对象, 无法移动");
        if (Cell[targetPosition.X, targetPosition.Y] is not null)
            throw new Exception("目标位置已有对象, 无法移动");
        Cell[targetPosition.X, targetPosition.Y] = Cell[sourecPosition.X, sourecPosition.Y];
        Cell[sourecPosition.X, sourecPosition.Y] = null;
    }
    public void Set(Sprite item, int row, int column)
    {
        Cell[row, column] = item;
    }
    public bool CheckBounds(int row, int column)
    {
        return row < 0 || row >= Cell.GetLength(0) || column < 0 || column >= Cell.GetLength(1);
    }
    public void Update(SpriteBatch sb)
    {
        ZeroCalculate();
    }
    RenderTarget2D screen;
    public void Draw(SpriteBatch sb)
    {
        screen = new(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
        _graphicsDevice.SetRenderTarget(screen);
        _graphicsDevice.Clear(Color.White);
        sb.Begin();
        sb.Draw(Map, Zero, Color.White);
        for (int y = 0; y < Cell.GetLength(1); y++)
            for (int x = 0; x < Cell.GetLength(0); x++)
            {
                var obj = Cell[x, y];
                if (obj is null) continue;
                Vector2 position = new()
                {
                    X = x * CellWidth + CellWidth / 2f + Zero.X,
                    Y = y * CellWidth + CellWidth / 2f + Zero.Y
                };
                // 绘制时以纹理中心为旋转中心
                DrawSB(sb, obj, position, 1 / SayoKNIGame.DRP);
            }
        sb.End();
        _graphicsDevice.SetRenderTarget(null);
        sb.Begin();
        sb.Draw(texture: screen,
                position: Vector2.Zero,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1,
                effects: SpriteEffects.None,
                layerDepth: 0f);
        sb.End();
        SceneManager.BackGround = screen;
    }

    private void ZeroCalculate()
    {
        int mapWidth = Cell.GetLength(0) * CellWidth;
        int mapHeight = Cell.GetLength(1) * CellWidth;
        Zero = new(_graphicsDevice.Viewport.Width  / 2 - mapWidth / 2,
                         _graphicsDevice.Viewport.Height  / 2 - mapHeight / 2);
    }
    private static void DrawSB(SpriteBatch sb, Sprite obj, Vector2 position, float scale)
    {
        sb.Draw(obj.CurrectTexture2D,
                position,
                null,
                Color.White,
                (float)obj.Status.Rotation,
                origin: new Vector2(obj.CurrectTexture2D.Width / 2f, obj.CurrectTexture2D.Height / 2f),
                scale: scale,
                SpriteEffects.None,
                layerDepth: 0f);
    }
}