using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sayo.Core.Scene
{
    public static class SceneManager
    {
        private static SayoGame _game;
        public static void Initialize(SayoGame game)
        {
            _game = game;
        }

        public static void ChangeScene(SceneBase newScene)
        {
            _game.ChangeScene(newScene);
        }
    }
}
