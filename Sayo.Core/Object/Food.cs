using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sayo.Core.Object
{
    /// <summary>
    /// 表示可以放置在网格上随机空位的食物对象。
    /// </summary>
    /// <remarks>
    /// <see cref="Food"/> 类扩展了 <see cref="Sprite"/> 类，用于表示
    /// 基于网格的环境中的食品。食物的位置是动态管理的，可以更新或设置
    /// 到网格内的新随机位置。
    /// </remarks>
    public class Food : Sprite
    {
        private readonly Random Random = new();

        public Food(Texture2D texture2D) : base(texture2D)
        {
            ObjType = ObjType.Food;
            Status.TargetPosition = Point.Zero;
        }

        public void Update(Grid grid)
        {
            Set(grid);
        }
        /// <summary>
        /// 在随机位置设置食物,设置成功返回true,失败返回false
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public bool Set(Grid grid)
        {
            List<(int x, int y)> emptyCells = [];

            for (int y = 0; y < grid.Cell.GetLength(1); y++)
            {
                for (int x = 0; x < grid.Cell.GetLength(0); x++)
                {
                    if (grid.Cell[x, y] == null)
                        emptyCells.Add((x, y));
                }
            }

            if (emptyCells.Count == 0)
                return false;

            // 随机取一个坐标
            (int xPos, int yPos) = emptyCells[Random.Next(emptyCells.Count)];

            // 把食物放进去
            grid.Cell[xPos, yPos] = this;
            // 最后再移除原来的食物
            grid.Cell[Status.TargetPosition.X, Status.TargetPosition.Y] = null;
            Status.TargetPosition = new Point(xPos, yPos);
            return true;
        }
    }
}