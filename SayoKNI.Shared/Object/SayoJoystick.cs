using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace SayoKNI.Object
{
    public class SayoJoystick
    {
        private static Texture2D _texture2D;
        private static Rectangle[] _rectangles;
        private Vector2 _position = Vector2.Zero;
        private Vector2 _centerOffset = Vector2.Zero;
        private bool _visible = false;
        private int _frame = 0;
        private float _radius = 0;
        public Keys Key { get; private set; }

        public SayoJoystick(Texture2D texture, Rectangle[] rectangle)
        {
            _texture2D ??= texture;
            _rectangles ??= rectangle;
            var jsbase = _rectangles[0];
            _radius = jsbase.Width / 2;
        }

        public void Update(GameTime gameTime, TouchLocation touchLocation, MouseState mouseState)
        {
            _centerOffset = Vector2.Zero;

            // 超时隐藏
            if (_frame > 60)
            {
                _visible = false;
                _position = Vector2.Zero;
            }

            // -------- 统一输入判断 --------
            bool isTouchActive =
                touchLocation.State == TouchLocationState.Pressed ||
                touchLocation.State == TouchLocationState.Moved;

            bool isMouseActive =
                mouseState.LeftButton == ButtonState.Pressed;

            if (!isTouchActive && !isMouseActive)
            {
                Key = Keys.None;
                _frame++;
                return;
            }

            _frame = 0;

            // 统一输入位置
            Vector2 inputPosition = isMouseActive
                ? new Vector2(mouseState.X, mouseState.Y)
                : touchLocation.Position;

            // -------- 首次激活 --------
            if (!_visible)
            {
                _visible = true;
                _position = inputPosition;
                Key = Keys.None;
                return;
            }

            // -------- 计算偏移 --------
            _centerOffset = inputPosition - _position;
            float lengthSq = _centerOffset.LengthSquared();

            float radiusSq = _radius * _radius;

            // 限制在圆内
            if (lengthSq > radiusSq)
            {
                _centerOffset.Normalize();
                _centerOffset *= _radius;
                lengthSq = radiusSq; // 已限制
            }

            // -------- 判断方向 --------
            float deadZoneSq = radiusSq * 0.25f; // 0.5^2

            if (lengthSq <= deadZoneSq)
            {
                Key = Keys.None;
                return;
            }

            float angle = MathF.Atan2(_centerOffset.Y, _centerOffset.X);

            if (angle >= -MathF.PI / 4 && angle < MathF.PI / 4)
                Key = Keys.Right;
            else if (angle >= MathF.PI / 4 && angle < 3 * MathF.PI / 4)
                Key = Keys.Down;
            else if (angle >= -3 * MathF.PI / 4 && angle < -MathF.PI / 4)
                Key = Keys.Up;
            else
                Key = Keys.Left;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_visible) return;
            spriteBatch.Begin();
            spriteBatch.Draw(_texture2D, _position,
                _rectangles[0], Color.White, 0f, new Vector2(_rectangles[0].Width / 2, _rectangles[0].Height / 2), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_texture2D, _position + _centerOffset,
                _rectangles[1], Color.White, 0f, new Vector2(_rectangles[1].Width / 2, _rectangles[1].Height / 2), 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}