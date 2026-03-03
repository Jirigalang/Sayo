using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SayoKNI;

public static class TextureManager
{
    public static Texture2D SayoHead;
    public static Texture2D SayoHeadEating;
    public static Texture2D SayoBody;
    public static Texture2D SayoBodyTurn;
    public static Texture2D SayoBodyFull;
    public static Texture2D SayoBodyTurnFull;
    public static Texture2D SayoButt;
    public static Texture2D Food;
    public static Texture2D Tile;
    public static Texture2D SayoTitle;
    public static Texture2D JoyStick;
    public static SpriteFont Font;

    public static void Initialize(Microsoft.Xna.Framework.Content.ContentManager content)
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
