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
    public struct PixelCoordinates
    {
        private decimal _x;
        public decimal X
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

        private decimal _y;
        public decimal Y
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