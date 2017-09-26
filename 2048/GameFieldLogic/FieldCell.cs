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

namespace _2048
{
    /// <summary>
    /// each cell of the game field has FieldCell type
    /// </summary>
    public class FieldCell
    {
        public Rectangle CellRectangle { get; set; }
        public bool IsEmpty { set; get; }

        public FieldCell(Rectangle cellRectangle)
        {
            CellRectangle = cellRectangle;
        }
    }
}