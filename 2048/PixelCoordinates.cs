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
    struct PixelCoordinates
    {
        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("PixelCoordinates should be greater than 0 and less than size of screen!");
                }
                _x = value;
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("PixelCoordinates should be greater than 0 and less than size of screen!");
                }
                _y = value;
            }
        }
    }
}