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
using Newtonsoft.Json;
using Org.Apache.Http.Impl.Conn;

namespace _2048
{
    public struct GameCoordinates
    {
        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                if (value >= 0 && value <= 2)
                {
                    _x = value;
                }
                else
                {
                    throw new ArgumentException("Game coordinates should be between 0 and 2!");
                }
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                if (value >= 0 && value <= 2)
                {
                    _y = value;
                }
                else
                {
                    throw new ArgumentException("Game coordinates should be between 0 and 2!");
                }
            }
        }

        private int _z;
        public int Z
        {
            get { return _z; }
            set
            {
                if (value >= 0 && value <= 2)
                {
                    _z = value;
                }
                else
                {
                    throw new ArgumentException("Game coordinates should be between 0 and 2!");
                }
            }
        }

        [JsonConstructor]
        public GameCoordinates(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public GameCoordinates Add(Vector2 direction)
        {
            if (direction.X == 1 && direction.Y == 1)
            {
                return new GameCoordinates(_x, _y, _z + 1);
            }
            if (direction.X == -1 && direction.Y == -1)
            {
                return new GameCoordinates(_x, _y, _z - 1);
            }
            int x = _x + (int)direction.X;
            int y = _y + (int)direction.Y;
            return new GameCoordinates(x,y,_z);
        }

        public static bool operator ==(GameCoordinates left, GameCoordinates right)
        {
            if (left.X == right.X &&
                left.Y == right.Y &&
                left.Z == right.Z)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(GameCoordinates left, GameCoordinates right)
        {
            if (left.X != right.X ||
                left.Y != right.Y ||
                left.Z != right.Z)
            {
                return true;
            }
            return false;
        }
    }
}