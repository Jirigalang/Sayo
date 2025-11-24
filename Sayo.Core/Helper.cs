using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals;
using Gum.Wireframe;
using System;

namespace Sayo.Core
{
    public static class Helper
    {
        public const double 不旋转 = 0;
        public const double 上方向 = 0;
        public const double 旋转90度 = Math.PI / 2;
        public const double 右方向 = Math.PI / 2;
        public const double 旋转180度 = Math.PI;
        public const double 下方向 = Math.PI;
        public const double 旋转270度 = Math.PI * 1.5;
        public const double 左方向 = Math.PI * 1.5;
        public static int Score = 0;
        public static Button CreateButton(Panel panel,EventHandler @event, string text, Anchor anchor, float width = 23, float height = 5, float textscale = 0.4f, float horizontalOffect = 0, float longitudinalOffset = 0)
        {
            Button button = new();
            button.Anchor(anchor);
            button.Text = text;
            var visual = (ButtonVisual)button.Visual;
            visual.Width = width;
            visual.Height = height;
            button.Visual.Y = longitudinalOffset;
            button.Visual.X = horizontalOffect;
            var textInstance = visual.TextInstance;
            textInstance.CustomFontFile = "Fonts/SayoFont.fnt";
            textInstance.UseCustomFont = true;
            visual.TextInstance.FontScale = textscale;
            button.Click += @event;
            panel.AddChild(button);
            return button;
        }
    }
    public class RandomBag
    {
        private readonly Random r = new();
        private readonly int[] bag;
        public int Current { get; private set; }

        public int Next => NextNumber();

        public RandomBag(int count)
        {
            bag = new int[count];
            for (int i = 0; i < count; i++)
            {
                bag[i] = i;
            }

        }

        public int NextNumber()
        {
            Current++;
            if (Current < bag.Length) return bag[Current];
            Shuffle();
            Current = 0;
            return bag[Current];
        }

        private void Shuffle()
        {
            for (int i = bag.Length - 1; i > 0; i--)
            {
                int j = r.Next(i + 1);
                (bag[j], bag[i]) = (bag[i], bag[j]);
            }
            Current = 0;
        }
    }
}
