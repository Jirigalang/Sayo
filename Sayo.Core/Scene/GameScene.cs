using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sayo.Core.Content.obj;
using System;
namespace Sayo.Core.Scene
{
    internal class GameScene : SceneBase
    {
        private Grid _grid;
        private SayoPlayer _sayo;
        public Food Food;
        private TimeSpan _moveTimer = TimeSpan.Zero;
        private readonly TimeSpan _moveInterval = TimeSpan.FromSeconds(0.25);
        private Keys lastKey = Keys.None;
        public static bool GameRunning = true;

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        {
            _sb = new(graphicsDevice);
            var head = content.Load<Texture2D>("SayoHead");
            var head_eatting = content.Load<Texture2D>("SayoHead_Eating");
            var body = content.Load<Texture2D>("SayoBody");
            var body_full = content.Load<Texture2D>("SayoBody_Full");
            var body_turn = content.Load<Texture2D>("SayoBody_Turn");
            var body_turn_full = content.Load<Texture2D>("SayoBody_Turn_Full");
            var bodys = new[] { body, body_full, body_turn, body_turn_full };
            var heads = new[] { head, head_eatting };
            var butt = content.Load<Texture2D>("SayoButt");
            var food = content.Load<Texture2D>("Food");
            var tile = content.Load<Texture2D>("Tile");
            _grid = new Grid();
            _grid.Initialize(graphicsDeviceManager, _sb, tile);
            _sayo = new SayoPlayer(heads, bodys, butt, _grid);
            Food = new Food(food);
            Food.Update(_grid);
        }
        public override void Draw(GameTime gameTime)
        {
            _sb.Begin();
            _grid.Draw(_sb);
            _sb.End();
        }
        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _moveTimer += gameTime.ElapsedGameTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                lastKey = Keys.Up;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                lastKey = Keys.Down;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                lastKey = Keys.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                lastKey = Keys.Left;
            }

            if (_moveTimer < _moveInterval) return;


            _moveTimer = TimeSpan.Zero;
            if (GameRunning)
            {
                _sayo.Update(lastKey, _grid, Food);
            }

            lastKey = Keys.None;
        }
        public override void Unload(ContentManager content)
        {
            _sb.Dispose();
        }
    }
}
