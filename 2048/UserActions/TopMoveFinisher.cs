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
    class TopMoveFinisher : IMoveFinisher
    {
        public Vector2 Direction
        {
            get
            {
                return new Vector2(0, -1);
            }
        }
        private List<Cell> _cells;

        private GameField _field;


        public void DeactivateFinishedCells()
        {
            foreach (var cell in _cells)
            {
                if (cell.CellRectangle == _field.FieldCells[cell.Coordinates.X, 0, cell.Coordinates.Z].CellRectangle)
                {
                    cell.Active = false;
                    cell.Coordinates = new GameCoordinates(cell.Coordinates.X, 0, cell.Coordinates.Z);
                    _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = false;
                    continue;
                }


                if (cell.Coordinates.Y > 0)
                {
                    if (!_field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y - 1, cell.Coordinates.Z].IsEmpty)
                    {
                        cell.Active = false;
                        _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = false;
                    }
                }
            }
        }

        public void ActivateFalselyFinishedCells()
        {
            foreach (var cell in _cells)
            {
                if (cell.Coordinates.Y > 0 && !cell.Active)
                {
                    if (_field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y - 1, cell.Coordinates.Z].IsEmpty)
                    {
                        cell.Active = true;
                        _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = true;
                    }
                }
            }
        }

        public TopMoveFinisher(List<Cell> cells, GameField field)
        {
            _cells = cells;
            _field = field;
        }
    }
}