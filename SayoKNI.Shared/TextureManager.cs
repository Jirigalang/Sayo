using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core;

namespace SayoKNI;

public static class TextureManager
{
    public static Texture2D SayoHead { get;private set; }
    public static Texture2D SayoHeadEating { get; private set; }
    public static Texture2D SayoBody { get; private set; }
    public static Texture2D SayoBodyTurn { get; private set; }
    public static Texture2D SayoBodyFull { get; private set; }
    public static Texture2D SayoBodyTurnFull { get; private set; }
    public static Texture2D SayoButt { get; private set; }
    public static Texture2D Food { get; private set; }
    public static Texture2D Tile { get; private set; }
    public static Texture2D SayoTitle { get; private set; }
    public static Texture2D JoyStick { get; private set; }
    public static SpriteFont Font { get; private set; }

    public static void Initialize(ContentManager content)
    {
        SayoHead = content.Load<Texture2D>("SayoHead");
        SayoHeadEating = content.Load<Texture2D>("SayoHead_Eating");
        SayoBody = content.Load<Texture2D>("SayoBody");
        SayoBodyTurn = content.Load<Texture2D>("SayoBody_Turn");
        SayoBodyFull = content.Load<Texture2D>("SayoBody_Full");
        SayoBodyTurnFull = content.Load<Texture2D>("SayoBody_Turn_Full");
        SayoButt = content.Load<Texture2D>("SayoButt");
        Food = content.Load<Texture2D>("Food");
        Tile = content.Load<Texture2D>("Tile");
        SayoTitle = content.Load<Texture2D>("SayoTitle");
        JoyStick = content.Load<Texture2D>(@"SayoJoystick");
        Font = content.Load<SpriteFont>("Fonts/Hud");
        SoundManager.Initialize(content);
    }
}
