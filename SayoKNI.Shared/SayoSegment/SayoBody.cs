using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static Sayo.Core.Helper;

namespace SayoKNI.SayoSegment;

internal class SayoBody : Segment
{
    public bool IsButt { get; set; } = false;
    public static Texture2D BodyTexture;
    public static Rectangle[] BodyRectangle;
    public SayoBody() : base(BodyTexture, BodyRectangle)
    {

    }
    public static void Initialize(Texture2D texture2D, Rectangle[] rectangles)
    {
        BodyTexture = texture2D;
        BodyRectangle = rectangles;
    }
    public void UpdateState(Segment lastSegment)
    {
        IsAte = lastSegment.IsAte;
    }
    public void UpdateLocation(Segment lastSegment)
    {
        Location.X = lastSegment.Direction switch
        {
            Keys.Up => Location.X,
            Keys.Down => Location.X,
            Keys.Left => Location.X - 1,
            Keys.Right => Location.X + 1,
            _ => Location.X
        };
        Location.Y = lastSegment.Direction switch
        {
            Keys.Up => Location.Y - 1,
            Keys.Down => Location.Y + 1,
            Keys.Left => Location.Y,
            Keys.Right => Location.Y,
            _ => Location.Y
        };
        Location = new Vector2((int)Location.X, (int)Location.Y);
        //TODO: 处理转向贴图和进食逻辑
        IsAte = lastSegment.IsAte;
    }

    public void UpdateDirection(Segment lastSegment)
    {

        (int, int) diff = ((int)(lastSegment.Location.X - Location.X), (int)(lastSegment.Location.Y - Location.Y));
        Keys targetDirection = diff switch
        {
            (0, 1) => Keys.Down,
            (0, -1) => Keys.Up,
            (1, 0) => Keys.Right,
            (-1, 0) => Keys.Left,
            (0, 0) => Direction, //没有移动，保持原来的方向
            _ => throw new Exception("Segment位置异常，无法计算方向")
        };
        //Direction对身体没有意义，属性保留给屁股使用
        switch (targetDirection)
        {
            case Keys.Up:

                switch (Direction)
                {
                    case Keys.Left:
                        Direction = Keys.Left;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 旋转90度;
                        break;
                    case Keys.Right:
                        Direction = Keys.Right;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 不旋转;
                        break;
                    default:
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.Common : BodyFrame.Common);
                        break;
                }
                break;
            case Keys.Down:
                switch (Direction)
                {
                    case Keys.Left:
                        Direction = Keys.Left;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 旋转180度;
                        break;
                    case Keys.Right:
                        Direction = Keys.Right;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 旋转270度;
                        break;
                    default:
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.Ate : BodyFrame.Common);
                        break;
                }
                break;
            case Keys.Left:
                switch (Direction)
                {
                    case Keys.Up:
                        Direction = Keys.Up;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 旋转270度;
                        break;
                    case Keys.Down:
                        Direction = Keys.Down;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 不旋转;
                        break;
                    default:
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.Ate : BodyFrame.Common);
                        break;
                }
                break;
            case Keys.Right:
                switch (Direction)
                {
                    case Keys.Up:
                        Direction = Keys.Up;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 旋转180度;
                        break;
                    case Keys.Down:
                        Direction = Keys.Down;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.AteTurned : BodyFrame.Turned);
                        Rolation = 旋转90度;
                        break;
                    case Keys.Left:
                        Direction = Keys.Left;
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.Ate : BodyFrame.Common);
                        break;
                    default:
                        Sprite.CurrentFrame = (int)(IsAte ? BodyFrame.Ate : BodyFrame.Common);
                        break;
                }
                break;
        }
    }

    public void UpdateButt()
    {
        if (!IsButt) return;
        //屁股不需要拐弯，所以直接根据方向设置贴图和旋转角度
        Rolation = Direction switch
        {
            Keys.Up => 不旋转,
            Keys.Right => 旋转90度,
            Keys.Down => 旋转180度,
            Keys.Left => 旋转270度,
            _ => 不旋转
        };
        Sprite.CurrentFrame = (int)BodyFrame.Butt;
    }

}
public enum BodyFrame
{
    Common,
    Ate,
    Turned,
    AteTurned,
    Butt
}