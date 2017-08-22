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
            for (int i = _cells.Count - 1; i > -1; i--)
            {
                GameCoordinates neighbourCoordinates = _cells[i].Coordinates.Add(direction);
                
                if (IsOutOfScope(neighbourCoordinates))
                {
                    continue;
                }

                if (_field.FieldCells[
                    neighbourCoordinates.X,
                    neighbourCoordinates.Y,
                    neighbourCoordinates.Z].IsEmpty == false)
                {
                    for (int j = _cells.Count - 1; j > -1; j--)
                    {
                        bool isThisThirdEqualCellInARow = false;

                        if (_cells[j] == _cells[i])
                        {
                            continue;
                        }
                        if (_cells[j].Coordinates == neighbourCoordinates)
                        {
                            if (_cells[j].Value == _cells[i].Value)
                            {
                                // TODO : change FieldCell.IsEmpty from bool to int
                                //this code is added to fix bug of twice handling the situation above
                                //for example 2,2,2 -> _,_,8 in one step, instead of _,2,4
                                foreach (var cell in _cells)
                                {
                                    if (cell.Coordinates == _cells[j].Coordinates.Add(direction) &&
                                        cell.Value == _cells[j].Value)
                                    {
                                        isThisThirdEqualCellInARow = true;
                                    }
                                }

                                if (isThisThirdEqualCellInARow == false)
                                {
                                    //uniting two cells
                                    _cells.Add(new Cell(_cells[j].Value*2, _cells[j].Coordinates));
                                    _cells[j].ForRemove = true;
                                    _cells[i].ForRemove = true;
                                    _field.FieldCells[
                                        _cells[i].Coordinates.X,
                                        _cells[i].Coordinates.Y,
                                        _cells[i].Coordinates.Z].IsEmpty = true;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = _cells.Count - 1; i > -1; i--)
            {
                if (_cells[i].ForRemove)
                    _cells.Remove(_cells[i]);
            }
        }

        private static bool IsOutOfScope(GameCoordinates coords)
        {
            if (coords.X < 0 || coords.X > 2 ||
                coords.Y < 0 || coords.Y > 2 ||
                coords.Z < 0 || coords.Z > 2)
            {
                return true;
            }
            return false;
        }
    }
}