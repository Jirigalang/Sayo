using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sayo.Core.Object;

namespace SayoKNI.SayoSegment;

public abstract class Segment : GameObject
{
    public bool IsAte;
    public Segment(Texture2D texture, Rectangle[] sourceRectangle) : base(texture, sourceRectangle)
    {
    }
}

public class GameObject
{
    public Sprite Sprite { get; set; }
    public Vector2 Location = new(1, 1);
    public Keys Direction = Keys.Right;
    public float Rolation = 0;

    public GameObject(Texture2D texture, Rectangle[] sourceRectangle)
    {
        Sprite = new(texture, sourceRectangle);
    }

    public Vector2 Position
    {
        get => new(Location.X * Grid.CellWidth, Location.Y * Grid.CellWidth);
    }

}