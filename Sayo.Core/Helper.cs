using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
    public class RandomBag
    {
        Random r = new();
        private readonly int[] bag;
        private int currentIndex = 0;
        public int Current { get => currentIndex; }
        public int Next { get => NextNumber(); }

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
            if (currentIndex >= bag.Length)
            {
                Shuffle();
                currentIndex = 0;
                return bag[currentIndex];
            }
            currentIndex++;
            return bag[currentIndex];
        }

        private void Shuffle()
        {
            for (int i = bag.Length - 1; i > 0; i--)
            {
                int j = r.Next(i + 1);
                (bag[j], bag[i]) = (bag[i], bag[j]);
            }
            currentIndex = 0;
        }
    }
}
