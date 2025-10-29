using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sayo.Core.Scene
{
    public static class SceneManager
    {
        public static SceneBase CurrentScene;
        private static GraphicsDevice GraphicsDevice;
        private static ContentManager Content;
        private static GraphicsDeviceManager GraphicsDeviceManager;
        public static Dictionary<string, SceneBase> Scenes;
        public static void Initialize(GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
            GraphicsDeviceManager = graphicsDeviceManager;
            Scenes = new Dictionary<string, SceneBase>
            {
                { "MainMenu", new MainMenuScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "Game", new GameScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "GameOver", new GameOverScene(GraphicsDevice, Content, GraphicsDeviceManager) },
                { "Credits", new CreditsScene(GraphicsDevice, Content, GraphicsDeviceManager) }
            };

        }

        public static void ChangeScene(string sceneName)
        {
            if (Scenes.TryGetValue(sceneName, out SceneBase value))
            {
                CurrentScene.Unload();
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
