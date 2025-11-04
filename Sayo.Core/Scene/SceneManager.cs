using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sayo.Core.Scene
{
    public static class SceneManager
    {
        private static GraphicsDevice GraphicsDevice;
        private static ContentManager Content;
        private static GraphicsDeviceManager GraphicsDeviceManager;
        private static Dictionary<string, SceneBase> _scenes;
        public static SceneBase CurrentScene { get; set; }
        public static void Initialize(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
            GraphicsDeviceManager = graphicsDeviceManager;
            _scenes = new Dictionary<string, SceneBase>
            {
                { "MainMenu", new MainMenuScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "Game", new GameScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "GameOver", new GameOverScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "Credits", new CreditsScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "Setting", new SettingScene(GraphicsDevice, Content, GraphicsDeviceManager) }
            };

        }

        public static void ChangeScene(string sceneName)
        {
            if (_scenes.TryGetValue(sceneName, out SceneBase value))
            {
                CurrentScene?.Unload();
                CurrentScene = value;
                CurrentScene.Load();
            }
            else
            {
                throw new Exception($"Scene '{sceneName}' not found.");
            }
        }
    }
}
