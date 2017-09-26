using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using _2048.DifficultyLevels;
using _2048.Saving;

namespace _2048.GameFieldLogic
{
    /// <summary>
    /// contains logic of keeping score,
    /// calculating, saving and loading best score
    /// </summary>
    public class Score
    {
        int _scoreValue;
        public int ScoreValue
        {
            get { return _scoreValue; }
            set
            {
                _scoreValue = value;
                if (_scoreValue > BestScore)
                {
                    BestScore = ScoreValue;
                }
            } 
        }

        public int BestScore { get; private set; }

        private static IScoreSaver _scoreSaver;

        [JsonConstructor]
        public Score(int scoreValue, int bestScore)
        {
            ScoreValue = scoreValue;
            BestScore = bestScore;
        }

        public Score()
        {
            _scoreSaver = Activity1.IoCContainer.GetInstance<IScoreSaver>();
        }

    
        public Score(Score copy)
        {
            ScoreValue = copy.ScoreValue;
            BestScore = copy.BestScore;
        }
        
        public void LoadBestScore(DifficultyLevel? difficultyLevel)
        {
            BestScore = _scoreSaver.LoadBestScore(difficultyLevel);          
        }

        public void SaveBestScore(DifficultyLevel? difficultyLevel)
        {
            _scoreSaver.SaveBestScore(difficultyLevel, BestScore);
        }
        
    }
}