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

namespace _2048.GameFieldLogic
{
    class CellsCombiner
    {
        GameField _field;

        List<Cell> _cells;

        public CellsCombiner(GameField field, List<Cell> cells)
        {
            _field = field;
            _cells = cells;
        }

        public void CombineCells(Vector2 direction)
        {
            foreach (var cell1 in _cells)
            {
                int x = cell1.Coordinates.X + (int) direction.X;
                int y = cell1.Coordinates.Y + (int) direction.Y;
                int z = cell1.Coordinates.Z + (int) (direction.X*direction.Y);

                if (_field.FieldCells[x,y,z].IsEmpty == false)
                {
                    foreach (var cell2 in _cells)
                    {
                        if (cell2 == cell1)
                        {
                            continue;
                        }
                        if (cell2.Coordinates.X == x &&
                            cell2.Coordinates.Y == y &&
                            cell2.Coordinates.Z == z)
                        {
                            if (cell2.Value == cell1.Value)
                            {
                                // TODO : change FieldCell.IsEmpty from bool to int
                                _cells.Remove(cell2);
                                _cells.Remove(cell1);
                               // _cells.Add(new Cell(cell2.Value * 2, cell2.Coordinates));
                            }
                        }
                    }
                }


            }
        }
    }
}