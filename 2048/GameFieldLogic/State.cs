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

namespace _2048.GameFieldLogic
{
    class State
    {
        public GameField Field;
        public List<Cell> Cells;
        public Score Score;

        public State(GameField field, List<Cell> cells, Score score)
        {
            Field = new GameField(field);
            Cells = new List<Cell>();
            foreach (var cell in cells)
            {
                Cells.Add(new Cell(cell.Value, cell.Coordinates));
            }

            Score = new Score(score);
        }
    }
}