using Microsoft.Xna.Framework.Graphics;

namespace Sayo.Core.Object;

public abstract class Sprite(Texture2D texture)
{
    public Texture2D CurrectTexture2D = texture;
    public SegmentStatus Status = new();
    public SegmentStatus OldStatus = new();
    public ObjType ObjType;

    /// <summary>
    ///     移动到新位置后, 将新位置赋值给旧位置
    /// </summary>
    /// <param name="grid"></param>
    public void Move(Grid grid)
    {
        grid.Move(Status.SourcePosition, Status.TargetPosition);
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public enum Turn
{
    NoTurn,
    右上,
    下右,
    左下,
    右下,

    下左,
    左上,
    上右,
    上左

}

public enum ObjType
{
    Air,
    Body,
    Food,
    Edge
}