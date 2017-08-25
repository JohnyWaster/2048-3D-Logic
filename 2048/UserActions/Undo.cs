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
using _2048.GameFieldLogic;

namespace _2048.UserActions
{
    class Undo : IMoveFinisher
    {
        public Vector2 Direction { get { return Vector2.Zero; } }
        public void DeactivateFinishedCells()
        {
            throw new NotImplementedException();
        }

        public void ActivateFalselyFinishedCells()
        {
            throw new NotImplementedException();
        }

        private static int _memoryLength = 10;

        private static List<State> _memory;


        static Undo()
        {
            _memory = new List<State>(_memoryLength);

            IsMemoryEmpty = true;
        }
        
        public static void SaveState(GameField field,List<Cell> cells,Score score)
        {
            if (_memory.Count == _memoryLength)
            {
                _memory.RemoveAt(0);
            }

            _memory.Add(new State(field, cells, score));

            IsMemoryEmpty = false;
        }

        public static void CleanMemory()
        {
            _memory = new List<State>(_memoryLength);

            IsMemoryEmpty = true;
        }

        public static bool IsMemoryEmpty { get; private set; }

        public void RestoreState(ref GameField field,ref List<Cell> cells,ref Score score)
        {
            if (IsMemoryEmpty)
            {
                return;
            }

            field = _memory[_memory.Count - 1].Field;
            cells = _memory[_memory.Count - 1].Cells;
            score = _memory[_memory.Count - 1].Score;

            _memory.RemoveAt(_memory.Count - 1);

            if (_memory.Count == 0)
            {
                IsMemoryEmpty = true;
            }
        }

    }
}