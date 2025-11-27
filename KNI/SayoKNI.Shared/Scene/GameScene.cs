using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Sayo.Core.Object;
using System;
namespace Sayo.Core.Scene
{
    internal class GameScene(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        : SceneBase(graphicsDevice, content, graphicsDeviceManager)
    {
        private Grid _grid;
        private SayoPlayer _sayo;
        private Food _food;
        private TimeSpan _moveTimer = TimeSpan.Zero;
        private readonly TimeSpan _moveInterval = TimeSpan.FromSeconds(0.25);
        private Keys lastKey = Keys.None;
        private Keys prevKey = Keys.None;
        private Panel _gamePanel;
        private Button _retryButton;
        public static bool GameRunning = true;



        public override void Load()
        {
            GameRunning = true;
            var head = Content.Load<Texture2D>("SayoHead");
            var head_eatting = Content.Load<Texture2D>("SayoHead_Eating");
            var body = Content.Load<Texture2D>("SayoBody");
            var body_full = Content.Load<Texture2D>("SayoBody_Full");
            var body_turn = Content.Load<Texture2D>("SayoBody_Turn");
            var body_turn_full = Content.Load<Texture2D>("SayoBody_Turn_Full");
            var bodys = new[] { body, body_full, body_turn, body_turn_full };
            var heads = new[] { head, head_eatting };
            var butt = Content.Load<Texture2D>("SayoButt");
            var food = Content.Load<Texture2D>("Food");
            var tile = Content.Load<Texture2D>("Tile");

            _grid = new Grid(10, 10);
            if (SB.IsDisposed)
            {
                SB = new SpriteBatch(GameGraphicsDevice);
            }
            _grid.Initialize(SB, tile, GameGraphicsDeviceManager);
            _food = new Food(food);
            _food.Update(_grid);
            _sayo = new SayoPlayer(heads, bodys, butt, _grid, _food);
            CreatePanel();
        }

        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.Clear(Color.White);
            SB.Begin();
            _grid.Draw(SB);
            SB.End();
            GumService.Default.Draw();
        }

        public override void DrawToRenderTarget()
        {
            RenderTarget2D backgroud = new(GameGraphicsDevice, GameGraphicsDevice.Viewport.Width, GameGraphicsDevice.Viewport.Height);
            GameGraphicsDevice.SetRenderTarget(backgroud);
            GameGraphicsDevice.Clear(Color.White);
            SB.Begin();
            _grid.Draw(SB);
            SB.End();
            GameGraphicsDevice.SetRenderTarget(null);
            SceneManager.BackGround = backgroud;
        }

        /// <summary>
        /// 获取键盘状态,在tick时将最后一次输入传入sayo的update中
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _grid.Update(SB);
            GumService.Default.Update(gameTime);
            _moveTimer += gameTime.ElapsedGameTime;
            if (!GameRunning) return;
            Keys[] directions = [Keys.Up, Keys.Down, Keys.Left, Keys.Right];

            var state = Keyboard.GetState();

            foreach (var key in directions)
            {
                if (!state.IsKeyDown(key)) continue;
                if (lastKey == Keys.None)
                    lastKey = key;
                else
                    prevKey = key;
            }

            if (_moveTimer < _moveInterval) return;
            if (lastKey == Keys.None)
            {
                lastKey = prevKey;
                prevKey = Keys.None;
            }

            _moveTimer = TimeSpan.Zero;


            _sayo.Update(gameTime, lastKey, _grid);
            lastKey = Keys.None;


        }

        public override void Unload()
        {
        }
        private void CreatePanel()
        {
            GumService.Default.Root.Children?.Clear();
            // Create a container to hold all of our buttons
            _gamePanel = new Panel();
            _gamePanel.Dock(Dock.Fill);
            _gamePanel.AddToRoot();


            var pauseButton = Helper.CreateButton(_gamePanel, PauseButton_Click, "||", Anchor.BottomRight, width: 23, height: 5, textscale: 0.4f);
            _retryButton = Helper.CreateButton(_gamePanel, HandleRetryClicked, "重试", Anchor.Bottom, width: 70);
            _retryButton.IsVisible = false;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            GameRunning = !GameRunning;
            _retryButton.IsVisible = !GameRunning;
        }
        private static void HandleRetryClicked(object sender, EventArgs e)
        {
            SceneManager.ChangeScene("Game");
        }
    }
}
