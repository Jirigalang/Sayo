using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameGum;
using Sayo.Core.Object;
using SayoKNI;
using SayoKNI.Object;
using System;
using System.Linq;
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
        private SayoJoystick joystick;
        public static bool GameRunning = true;



        private bool _isInitialized = false;

        public override void Load()
        {
            GameRunning = true;
            if (SB.IsDisposed)
            {
                SB = new SpriteBatch(GameGraphicsDevice);
            }

            if (!_isInitialized)
            {
                InitializeGameObjects();
                _isInitialized = true;
            }
            else
            {
                ResetGameState();
            }

            // 每次加载时都创建 UI（因为 Unload 时会清除）
            CreatePanel();
            if (_retryButton != null)
            {
                _retryButton.IsVisible = !GameRunning;
            }
        }

        private void InitializeGameObjects()
        {
            joystick = new SayoJoystick(TextureManager.JoyStick, [new Rectangle(0, 0, 128, 128), new Rectangle(128, 0, 128, 128)]);
            var head = TextureManager.SayoHead;
            var head_eating = TextureManager.SayoHeadEating;
            var body = TextureManager.SayoBody;
            var body_full = TextureManager.SayoBodyFull;
            var body_turn = TextureManager.SayoBodyTurn;
            var body_turn_full = TextureManager.SayoBodyTurnFull;
            var bodys = new[] { body, body_full, body_turn, body_turn_full };
            var heads = new[] { head, head_eating };
            var butt = TextureManager.SayoButt;
            var food = TextureManager.Food;
            var tile = TextureManager.Tile;

            _grid = new Grid(10, 10);
            _grid.Initialize(SB, tile, GameGraphicsDeviceManager);
            _food = new Food(food);
            _food.Update(_grid);
            _sayo = new SayoPlayer(heads, bodys, butt, _grid, _food);
        }

        private void ResetGameState()
        {
            // 清空网格中的所有对象
            for (int y = 0; y < _grid.Cell.GetLength(1); y++)
                for (int x = 0; x < _grid.Cell.GetLength(0); x++)
                    _grid.Cell[x, y] = null;

            // 重置蛇和食物
            _sayo.Reset(_grid, _food);
            _food.Update(_grid);

            // 将蛇的各个部分放回网格
            _grid.Cell[_sayo.Head.Status.TargetPosition.X, _sayo.Head.Status.TargetPosition.Y] = _sayo.Head;
            for (int i = 0; i < SayoPlayer.bodyCount; i++)
            {
                if (_sayo.Bodys[i] != null)
                    _grid.Cell[_sayo.Bodys[i].Status.TargetPosition.X, _sayo.Bodys[i].Status.TargetPosition.Y] = _sayo.Bodys[i];
            }
            _grid.Cell[_sayo.Butt.Status.TargetPosition.X, _sayo.Butt.Status.TargetPosition.Y] = _sayo.Butt;

            lastKey = Keys.None;
            prevKey = Keys.None;
            _moveTimer = TimeSpan.Zero;
        }

        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.Clear(Color.White);
            _grid.Draw(SB);
            joystick.Draw(SB);
            GumService.Default.Draw();
        }

        /// <summary>
        /// 获取键盘状态,在tick时将最后一次输入传入sayo的update中
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _grid.Update(SB);
            var curMouse = Mouse.GetState();
            TouchCollection touches = TouchPanel.GetState();
            joystick.Update(gameTime, touches.FirstOrDefault(), curMouse);
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
            if (joystick.Key != Keys.None)
            {
                if (lastKey == Keys.None)
                    lastKey = joystick.Key;
                else
                    prevKey = joystick.Key;
            }
            if (_moveTimer < _moveInterval) return;
            if (lastKey == Keys.None)
            {
                lastKey = prevKey;
                prevKey = Keys.None;
            }

            _moveTimer = TimeSpan.Zero;


            _sayo.Update(gameTime, lastKey);
            lastKey = Keys.None;
        }

        public override void Unload()
        {
            // 只清理UI元素和事件处理器，不销毁游戏对象
            // 游戏对象会在整个程序生命周期中保持，避免重复创建
            if (_gamePanel != null)
            {
                GumService.Default.Root.Children?.Clear();
            }
        }
        private void CreatePanel()
        {
            GumService.Default.Root.Children?.Clear();
            // Create a container to hold all of our buttons
            _gamePanel = new Panel();
            _gamePanel.Dock(Dock.Fill);
            _gamePanel.AddToRoot();


            var pauseButton = Helper.CreateButton(_gamePanel, PauseButton_Click, "||", Anchor.BottomRight, width: 23, height: 5, textscale: 0.4f);
            _retryButton = Helper.CreateButton(_gamePanel, HandleRetryClicked, "Try Again", Anchor.Bottom, width: 70);
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
