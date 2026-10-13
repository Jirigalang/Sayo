using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sayo.Core;
using System;
using System.Collections.Generic;

namespace SayoKNI;

public static class TextureManager
{
    private static readonly List<Texture2D> sayoBody = [];
    private static readonly List<Texture2D> sayoBodyTurn = [];
    private static readonly List<Texture2D> sayoBodyFull = [];
    private static readonly List<Texture2D> sayoBodyTurnFull = [];
    private static readonly List<Texture2D> sayoButt = [];
    private static readonly List<Texture2D> food = [];

    public static Texture2D SayoHead { get; private set; }
    public static Texture2D SayoHeadEating { get; private set; }
    public static Texture2D SayoHeadDead { get; private set; }
    public static Texture2D SayoBody { get => sayoBody[Random.Shared.Next(sayoBody.Count)]; private set => sayoBody.Add(value); }
    public static Texture2D SayoBodyTurn { get => sayoBodyTurn[Random.Shared.Next(sayoBodyTurn.Count)]; private set => sayoBodyTurn.Add(value); }
    public static Texture2D SayoBodyFull { get => sayoBodyFull[Random.Shared.Next(sayoBodyFull.Count)]; private set => sayoBodyFull.Add(value); }
    public static Texture2D SayoBodyTurnFull { get => sayoBodyTurnFull[Random.Shared.Next(sayoBodyTurnFull.Count)]; private set => sayoBodyTurnFull.Add(value); }
    public static Texture2D SayoButt { get => sayoButt[Random.Shared.Next(sayoButt.Count)]; private set => sayoButt.Add(value); }
    public static Texture2D Food { get => food[Random.Shared.Next(food.Count)]; private set => food.Add(value); }
    public static Texture2D Tile { get; private set; }
    public static Texture2D SayoTitle { get; private set; }
    public static Texture2D JoyStick { get; private set; }
    public static SpriteFont Font { get; private set; }

    public static void Initialize(ContentManager content)
    {
        SayoHead = content.Load<Texture2D>("SayoHead");
        SayoHeadEating = content.Load<Texture2D>("SayoHead_Eating");

        SayoBody = content.Load<Texture2D>("SayoBody");
        SayoBody = content.Load<Texture2D>("SayoBody1");
        SayoBodyTurn = content.Load<Texture2D>("SayoBody_Turn");
        SayoBodyFull = content.Load<Texture2D>("SayoBody_Full");
        SayoBodyTurnFull = content.Load<Texture2D>("SayoBody_Turn_Full");
        SayoBodyTurn = content.Load<Texture2D>("SayoBody_Turn1");
        SayoBodyFull = content.Load<Texture2D>("SayoBody_Full1");
        SayoBodyTurnFull = content.Load<Texture2D>("SayoBody_Turn_Full1");

        SayoButt = content.Load<Texture2D>("SayoButt");
        SayoHeadDead = content.Load<Texture2D>("SayoHead_Dead");
        Food = content.Load<Texture2D>("Food");
        Tile = content.Load<Texture2D>("Tile");
        SayoTitle = content.Load<Texture2D>("SayoTitle");
        JoyStick = content.Load<Texture2D>(@"SayoJoystick");
        Font = content.Load<SpriteFont>("Fonts/Hud");
        SoundManager.Initialize(content);
    }
}
