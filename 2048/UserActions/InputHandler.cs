using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace _2048.UserActions
{
    static class InputHandler
    {
        static TouchLocation _prevLocation;

        static TouchLocation _currentLocation;

        static TouchCollection _touches;

        public static Vector2 GetUserAction()
        {
            // we use raw touch points for selection, since they are more appropriate
            // for that use than gestures. so we need to get that raw touch data.
            _touches = TouchPanel.GetState();

            // see if we have a new primary point down. when the first touch
            // goes down, we do hit detection to try and select one of our sprites.
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Pressed)
            {
                _prevLocation = _touches[0];
                return Vector2.Zero;
            }
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Released)
            {
                _currentLocation = _touches[0];

                // get your delta
                var delta = _currentLocation.Position - _prevLocation.Position;

                // Usually you don't want to do something if the user drags 1 pixel.
                if (delta.LengthSquared() < 30)
                    return Vector2.Zero;

                if (IsLeftTopDiagonal(delta))
                {
                    return new Vector2(-1, -1);
                }
                if (IsRightBottomDiagonal(delta))
                {
                    return new Vector2(1, 1);
                }
                if (IsHorizontalLeft(delta))
                {
                    return new Vector2(-1, 0);
                }
                if (IsHorizontalRight(delta))
                {
                    return new Vector2(1, 0);
                }
                if (IsVerticalBottom(delta))
                {
                    return new Vector2(0, 1);
                }
                if (IsVerticalTop(delta))
                {
                    return new Vector2(0, -1);
                }
            }
            return Vector2.Zero;
        }


        private static bool IsVerticalTop(Vector2 delta)
        {
            if (delta.Y < 0 && Math.Abs(delta.Y) > Math.Abs(delta.X))
            {
                return true;
            }

            return false;
        }

        private static bool IsVerticalBottom(Vector2 delta)
        {
            if (delta.Y > 0 && Math.Abs(delta.Y) > Math.Abs(delta.X))
            {
                return true;
            }

            return false;
        }

        private static bool IsHorizontalLeft(Vector2 delta)
        {
            if (delta.X < 0 && Math.Abs(delta.X) > Math.Abs(delta.Y))
            {
                return true;
            }

            return false;
        }

        private static bool IsHorizontalRight(Vector2 delta)
        {
            if (delta.X > 0 && Math.Abs(delta.X) > Math.Abs(delta.Y))
            {
                return true;
            }

            return false;
        }

        private static bool IsLeftTopDiagonal(Vector2 delta)
        {
            if (delta.X < 0 && delta.Y < 0 && IsDiagonal(delta))
            {
                return true;
            }

            return false;
        }

        private static bool IsRightBottomDiagonal(Vector2 delta)
        {
            if (delta.X > 0 && delta.Y > 0 && IsDiagonal(delta))
            {
                return true;
            }

            return false;
        }

        private static bool IsDiagonal(Vector2 delta)
        {
            if (Math.Abs(delta.X) > Math.Abs(delta.Y) / 1.7 && Math.Abs(delta.X) < Math.Abs(delta.Y) * 1.7)
            {
                return true;
            }
            return false;
        }
    }
}