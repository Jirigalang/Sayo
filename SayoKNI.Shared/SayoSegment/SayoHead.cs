using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sayo.Core.Object;
using static Sayo.Core.Helper;

namespace SayoKNI.SayoSegment;
internal class SayoHead : Segment
{
    public SayoHead(Texture2D texture, Rectangle[] soureceRectangle) : base(texture, soureceRectangle)
    {
        Location = new Vector2(3, 1);
    }
    public void UpdateLocation(Keys lastKey)
    {
        Location.X = Direction switch
        {
            Keys.Up => Location.X,
            Keys.Down => Location.X,
            Keys.Left => Location.X - 1,
            Keys.Right => Location.X + 1,
            _ => Location.X
        };
        Location.Y = Direction switch
        {
            Keys.Up => Location.Y - 1,
            Keys.Down => Location.Y + 1,
            Keys.Left => Location.Y,
            Keys.Right => Location.Y,
            _ => Location.Y
        };
        if (Location.X % 100 != 0 && Location.Y % 100 != 0) return;
        Direction = lastKey;
        Location = new Vector2(Location.X / 100, Location.Y / 100);
        Rolation = Direction switch
        {
            Keys.Up => 不旋转,
            Keys.Right => 旋转90度,
            Keys.Down => 旋转180度,
            Keys.Left => 旋转270度,
            _ => 不旋转
        };
    }
    public bool SetState(Food food)
    {
        Sprite.CurrentFrame = (int)(food.Location == Location ? HeadFrame.Eating : HeadFrame.Normal);
        IsAte = food.Location == Location;
        return IsAte;
    }
}
enum HeadFrame
{
    Eating,
    Normal
}