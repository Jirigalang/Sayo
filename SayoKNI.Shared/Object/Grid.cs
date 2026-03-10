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
public class Grid : IDisposable
{
    private int _row = 10;
    private int _column = 10;
    public Sprite[,] Cell;
    private const int _cellWidth = 64;
    public int CellWidth { get { return _cellWidth / (int)SayoKNIGame.DRP; } }
    private Vector2 Zero = Vector2.Zero;
    public RenderTarget2D Map;
    private static GraphicsDevice _graphicsDevice;
    private Texture2D _tile;
    public Grid()
    {
    }
    public void Initialize(SpriteBatch sb, Texture2D tile, GraphicsDeviceManager graphicsDeviceManager = null)
    {
        _graphicsDevice ??= graphicsDeviceManager.GraphicsDevice;
        _tile = tile;
        int maxRow = _graphicsDevice.Viewport.Width / CellWidth;
        int maxColumn = _graphicsDevice.Viewport.Height / CellWidth;
        if (_row > maxRow)
            _row = maxRow;
        if (_column > maxColumn)
            _column = maxColumn;
        Cell = new Sprite[_row, _column];
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

    // 资源释放状态标志
    private bool _disposed = false;

    // 实现标准的可继承释放模式，替代错误的 override Dispose()
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Unload()
    {
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // 释放托管资源
            try
            {
                Map?.Dispose();
            }
            catch { /* 忽略释放时的异常以保证稳定性 */ }
            Map = null;

            try
            {
                screen?.Dispose();
            }
            catch { }
            screen = null;

            // 如果还有其它托管可释放资源，也应在此处释放
        }

        // 释放非托管资源（若有）在此添加

        _disposed = true;
    }

    ~Grid()
    {
        Dispose(false);
    }

    RenderTarget2D screen;
    public void Draw(SpriteBatch sb)
    {
        screen ??= new(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
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
        Zero = new(_graphicsDevice.Viewport.Width / 2 - mapWidth / 2,
                         _graphicsDevice.Viewport.Height / 2 - mapHeight / 2);
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