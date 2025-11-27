using System;
using Microsoft.Xna.Framework;

namespace Sayo.Core.Object
{
    public struct SegmentStatus
    {
        public static readonly Point InitializationPosition = new(2, 1);
        public Point SourcePosition = new(InitializationPosition.X, InitializationPosition.Y);
        public Point TargetPosition = new(InitializationPosition.X + 1, InitializationPosition.Y);
        public Direction Direction = Direction.Right;
        public Turn Turn = Turn.NoTurn;
        public double Rotation = Math.PI / 2;
        public bool Ate = false;

        public SegmentStatus()
        {
        }
    }
}
