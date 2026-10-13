using ImGuiNET;
using Microsoft.Xna.Framework;
using Sayo.Core.Scene;
using System.Threading;

namespace SayoKNI
{
    /// <summary>
    /// ImGui 调试管理器单例类
    /// 用于集中管理所有ImGui相关的调试UI逻辑
    /// </summary>
#if DEBUG
    internal sealed class ImGuiDebugManager
    {
        private static ImGuiDebugManager _instance;
#if BLAZORGL
        private static readonly object _lockObject = new();
#else
        private static readonly Lock _lockObject = new();
#endif

        // 注册的数据源
        private GameScene _gameScene;
        private GameTime _gameTime;
        private bool _isVisible = true;

        public static ImGuiDebugManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        _instance ??= new ImGuiDebugManager();
                    }
                }
                return _instance;
            }
        }

        private ImGuiDebugManager()
        {
        }

        /// <summary>
        /// 注册游戏场景引用
        /// </summary>
        public void RegisterGameScene(GameScene gameScene)
        {
            _gameScene = gameScene;
        }

        /// <summary>
        /// 注册游戏时间对象
        /// </summary>
        public void RegisterGameTime(GameTime gameTime)
        {
            _gameTime = gameTime;
        }

        /// <summary>
        /// 切换调试面板的显示/隐藏
        /// </summary>
        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
        }

        /// <summary>
        /// 设置调试面板的可见性
        /// </summary>
        public void SetVisibility(bool visible)
        {
            _isVisible = visible;
        }

        /// <summary>
        /// 获取调试面板的可见性
        /// </summary>
        public bool IsVisible => _isVisible;

        /// <summary>
        /// 渲染调试UI面板
        /// 应该在ImGuiRenderer.BeforeLayout() 之后、ImGuiRenderer.AfterLayout() 之前调用
        /// </summary>
        public void Render()
        {
            if (!_isVisible)
                return;

            ImGui.SetNextWindowPos(new System.Numerics.Vector2(10, 10), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 400), ImGuiCond.FirstUseEver);

            if (ImGui.Begin("🎮 Sayo Debug Panel", ref _isVisible, ImGuiWindowFlags.AlwaysAutoResize))
            {
                // 场景信息
                ImGui.Text($"Current Scene: {_gameScene?.GetType().Name ?? "None"}");
                ImGui.Separator();

                // 检查是否在游戏场景中
                if (_gameScene != null)
                {
                    DrawGameDebugInfo();
                }
                else
                {
                    ImGui.TextColored(new System.Numerics.Vector4(1, 1, 0, 1), "Not in GameScene");
                }

                ImGui.Separator();

                // 快捷键提示
                ImGui.TextDisabled("Press F1 to toggle this panel");

                ImGui.End();
            }
        }

        /// <summary>
        /// 绘制游戏调试信息
        /// </summary>
        private void DrawGameDebugInfo()
        {
            // 游戏状态
            ImGui.TextColored(new System.Numerics.Vector4(0, 1, 0, 1), "=== Game Status ===");
            ImGui.Text($"Game Running: {(GameScene.GameRunning ? "✓" : "✗")}");
            ImGui.Text($"Move Timer: {_gameScene.MoveTimer.TotalSeconds:F2}s");

            ImGui.Separator();

            // 蛇的信息
            ImGui.TextColored(new System.Numerics.Vector4(1, 0.5f, 0, 1), "=== Snake Info ===");
            if (_gameScene.Sayo != null)
            {
                var snakeHead = _gameScene.Sayo.Head;
                var snakeBodyCount = Sayo.Core.Object.SayoPlayer.bodyCount;

                ImGui.Text($"Score: {_gameScene.Sayo.Score}");
                ImGui.Text($"Body Count: {snakeBodyCount}");
                ImGui.Text($"Last Direction: {_gameScene.LastDirection}");

                if (snakeHead?.Status != null)
                {
                    var headPos = snakeHead.Status.TargetPosition;
                    ImGui.Text($"Head Position: ({headPos.X}, {headPos.Y})");
                }
            }

            ImGui.Separator();

            // 食物信息
            ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "=== Food Info ===");
            if (_gameScene.Food != null)
            {
                var foodPos = _gameScene.Food.Status.TargetPosition;
                ImGui.Text($"Food Position: ({foodPos.X}, {foodPos.Y})");
            }

            ImGui.Separator();

            // 网格信息
            ImGui.TextColored(new System.Numerics.Vector4(0, 0.5f, 1, 1), "=== Grid Info ===");
            if (_gameScene.Grid != null)
            {
                var gridRows = _gameScene.Grid.Cell.GetLength(0);
                var gridCols = _gameScene.Grid.Cell.GetLength(1);
                var cellSize = Sayo.Core.Object.Grid.CellWidth;

                ImGui.Text($"Grid Size: {gridRows} x {gridCols}");
                ImGui.Text($"Cell Width: {cellSize}");
            }

            ImGui.Separator();

            // 性能信息
            ImGui.TextColored(new System.Numerics.Vector4(0.8f, 0.2f, 0.8f, 1), "=== Performance ===");
            if (_gameTime != null)
            {
                var fps = 1000.0 / _gameTime.ElapsedGameTime.TotalMilliseconds;
                ImGui.Text($"FPS: {fps:F1}");
            }
            ImGui.Text($"DRP: {SayoKNIGame.DRP}");
        }

        /// <summary>
        /// 清理资源（可选）
        /// </summary>
        public void Dispose()
        {
            _gameScene = null;
            _gameTime = null;
        }
    }
#else
    /// <summary>
    /// Release 模式下的空实现
    /// </summary>
    internal sealed class ImGuiDebugManager
    {
        private static ImGuiDebugManager _instance = new ImGuiDebugManager();

        public static ImGuiDebugManager Instance => _instance;

        public void RegisterGameScene(GameScene gameScene) { }
        public void RegisterGameTime(GameTime gameTime) { }
        public void ToggleVisibility() { }
        public void SetVisibility(bool visible) { }
        public bool IsVisible => false;
        public void Render() { }
        public void Dispose() { }
    }
#endif
    }
