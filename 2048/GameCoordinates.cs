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

namespace _2048
{
    struct GameCoordinates
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

        public GameCoordinates(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    }
}