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
using _2048.DifficultyLevels;

namespace _2048.UserActions
{
    /// <summary>
    /// in this class all logic of user input is implemented.
    /// </summary>
    class InputHandler
    {
        TouchLocation _prevLocation;

        TouchLocation _currentLocation;

        TouchCollection _touches;

        BottomMoveFinisher _bottomMoveFinisher;

        LeftMoveFinisher _leftMoveFinisher;

        RightMoveFinisher _rightMoveFinisher;

        TopMoveFinisher _topMoveFinisher;

        LeftTopDiagonalFinisher _leftTopDiagonal;

        RightBottomDiagonalFinisher _rightBottomDiagonal;

        List<Cell> _cells;

        GameField _field; 

        static event Action _restart;

        static event Action _undo; 

        public InputHandler(List<Cell> cells, GameField field)
        {
            _field = field;
            _cells = cells;


            _bottomMoveFinisher = new BottomMoveFinisher(_cells, _field);
            _leftMoveFinisher = new LeftMoveFinisher(_cells, _field);
            _rightMoveFinisher = new RightMoveFinisher(_cells, _field);
            _topMoveFinisher = new TopMoveFinisher(_cells, _field);
            _leftTopDiagonal = new LeftTopDiagonalFinisher(_cells, _field);
            _rightBottomDiagonal = new RightBottomDiagonalFinisher(_cells, _field);
        }

        /// <summary>
        /// Method for handling user gesture during gameplay
        /// </summary>
        /// <returns></returns>
        public IMoveFinisher GetUserAction()
        {
            // we use raw touch points for selection, since they are more appropriate
            // for that use than gestures. so we need to get that raw touch data.
            _touches = TouchPanel.GetState();
            
            // see if we have a new primary point down. when the first touch
            // goes down, we do hit detection to try and select one of our sprites.
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Pressed)
            {
                _prevLocation = _touches[0];
                return null;
            }
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Released)
            {
                _currentLocation = _touches[0];

                // get your delta
                var delta = _currentLocation.Position - _prevLocation.Position;

                // Usually you don't want to do something if the user drags 1 pixel.
                if (delta.LengthSquared() < 30)
                {
                    if (GameField.UndoButton.Contains(_currentLocation.Position) &&
                        GameField.UndoButton.Contains(_prevLocation.Position))
                    {
                        _undo.Invoke();
                    }
                    if (GameField.RestartButton.Contains(_currentLocation.Position) &&
                        GameField.RestartButton.Contains(_prevLocation.Position))
                    {
                        _restart.Invoke();
                    }
                    return null;
                }
                    

                if (IsLeftTopDiagonal(delta))
                {
                    return _leftTopDiagonal;
                }
                if (IsRightBottomDiagonal(delta))
                {
                    return _rightBottomDiagonal;
                }
                if (IsHorizontalLeft(delta))
                {
                    return _leftMoveFinisher;
                }
                if (IsHorizontalRight(delta))
                {
                    return _rightMoveFinisher;
                }
                if (IsVerticalBottom(delta))
                {
                    return _bottomMoveFinisher;
                }
                if (IsVerticalTop(delta))
                {
                    return _topMoveFinisher;
                }
            }
            return null;
        }

        /// <summary>
        /// Method which determines if user pushed try again button
        /// </summary>
        /// <returns></returns>
        public bool TryAgain()
        {
            // we use raw touch points for selection, since they are more appropriate
            // for that use than gestures. so we need to get that raw touch data.
            _touches = TouchPanel.GetState();
            
            // see if we have a new primary point down. when the first touch
            // goes down, we do hit detection to try and select one of our sprites.
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Pressed)
            {
                _prevLocation = _touches[0];
                return false;
            }
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Released)
            {
                _currentLocation = _touches[0];

                // get your delta
                var delta = _currentLocation.Position - _prevLocation.Position;

                // Usually you don't want to do something if the user drags 1 pixel.
                if (delta.LengthSquared() < 30)
                {
                    if (GameField.UndoButton.Contains(_currentLocation.Position) &&
                        GameField.UndoButton.Contains(_prevLocation.Position))
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public DifficultyLevel? GetDifficultyLevel(FirstScreen firstScreen)
        {
            // we use raw touch points for selection, since they are more appropriate
            // for that use than gestures. so we need to get that raw touch data.
            _touches = TouchPanel.GetState();

            // see if we have a new primary point down. when the first touch
            // goes down, we do hit detection to try and select one of our sprites.
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Pressed)
            {
                _prevLocation = _touches[0];
                return null;
            }
            if (_touches.Count > 0 && _touches[0].State == TouchLocationState.Released)
            {
                _currentLocation = _touches[0];

                // get your delta
                var delta = _currentLocation.Position - _prevLocation.Position;

                // Usually you don't want to do something if the user drags 1 pixel.
                if (delta.LengthSquared() < 30)
                {
                    if (firstScreen.HardButton.Contains(_currentLocation.Position) &&
                        firstScreen.HardButton.Contains(_prevLocation.Position))
                    {
                        return DifficultyLevel.VeryHard;
                    }

                    if (firstScreen.EasyButton.Contains(_currentLocation.Position) &&
                         firstScreen.EasyButton.Contains(_prevLocation.Position))
                    {
                        return DifficultyLevel.Easy;
                    }
                }
            }
            return null;
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

        public static void AddRestartHandler(Action handler)
        {
            _restart += handler;
        }


        public static void AddUndoHandler(Action handler)
        {
            _undo += handler;
        }
    }
}