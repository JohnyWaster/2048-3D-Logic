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
using _2048.DifficultyLevels;
using _2048.Saving;

namespace _2048.GameFieldLogic
{
    public class State
    {
        public GameField Field;
        public List<Cell> Cells;
        public Score Score;
        public DifficultyLevel? DifficultyLevel;

        private IGameStateSaver _gameStateSaver = Activity1.IoCContainer.GetInstance<IGameStateSaver>();


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

        public State(GameField field, List<Cell> cells, Score score, DifficultyLevel? difficultyLevel)
        {
            Field = new GameField(field);
            Cells = new List<Cell>();
            foreach (var cell in cells)
            {
                Cells.Add(new Cell(cell.Value, cell.Coordinates));
            }

            Score = new Score(score);

            DifficultyLevel = difficultyLevel;
        }

        public void SaveGameState()
        {
            _gameStateSaver.SaveGameState(this);
        }

        public State LoadGameState()
        {
            return _gameStateSaver.LoadGameState();
        }
    }
}