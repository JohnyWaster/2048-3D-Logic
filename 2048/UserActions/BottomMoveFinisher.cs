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

namespace _2048.UserActions
{
    class BottomMoveFinisher : IMoveFinisher
    {
        public Vector2 Direction
        {
            get
            {
                return new Vector2(0, 1);
            }
        }

        private List<Cell> _cells;

        private GameField _field;


        public void DeactivateFinishedCells()
        {
            foreach (var cell in _cells)
            {
                if (cell.CellRectangle == _field.FieldCells[cell.Coordinates.X, 2, cell.Coordinates.Z].CellRectangle)
                {
                    cell.Active = false;
                    cell.Coordinates = new GameCoordinates(cell.Coordinates.X, 2, cell.Coordinates.Z);
                    _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = false;
                    continue;
                }

                if (cell.Coordinates.Y < 2)
                {
                    if (!_field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y + 1, cell.Coordinates.Z].IsEmpty)
                    {
                        cell.Active = false;
                        _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = false;
                    }
                }
            }
        }

        public BottomMoveFinisher(List<Cell> cells, GameField field)
        {
            _cells = cells;
            _field = field;
        }
    }
}