﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2048.UserActions
{
    class RightMoveFinisher : IMoveFinisher
    {
        public Vector2 Direction
        {
            get
            {
                return new Vector2(1,0);
            }
        }

        private List<Cell> _cells;

        private GameField _field;


        public void DeactivateFinishedCells()
        {
            foreach (var cell in _cells)
            {
                if (cell.CellRectangle == _field.FieldCells[2, cell.Coordinates.Y, cell.Coordinates.Z].CellRectangle)
                {
                    cell.Active = false;
                    cell.Coordinates = new GameCoordinates(2, cell.Coordinates.Y, cell.Coordinates.Z);
                    _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = false;
                    continue;
                }

                if (cell.Coordinates.X < 2)
                {
                    if (!_field.FieldCells[cell.Coordinates.X + 1, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty)
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
                if (cell.Coordinates.X < 2 && !cell.Active)
                {
                    if (_field.FieldCells[cell.Coordinates.X + 1, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty)
                    {
                        cell.Active = true;
                        _field.FieldCells[cell.Coordinates.X, cell.Coordinates.Y, cell.Coordinates.Z].IsEmpty = true;
                    }
                }
            }
        }

        public RightMoveFinisher(List<Cell> cells, GameField field)
        {
            _cells = cells;
            _field = field;
        }
    }
}